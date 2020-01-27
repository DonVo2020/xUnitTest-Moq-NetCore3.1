using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using DonVo.Inventory.Infrastructures.Helpers;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels;
using DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories
{
    public class MaterialRequestNoteRepository : IMaterialRequestNoteRepository
    {
        private const string UserAgent = "inventory-service";
        protected DbSet<MaterialsRequestNote> _dbSet;
        public IIdentityInterface _identityInterface;
        public readonly IServiceProvider _serviceProvider;
        public InventoryDbContext _inventoryDbContext;

        public MaterialRequestNoteRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            _inventoryDbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dbSet = dbContext.Set<MaterialsRequestNote>();
            _identityInterface = serviceProvider.GetService<IIdentityInterface>();
        }

        public class SppParams
        {
            public string Context { get; set; }
            public string Id { get; set; }
            public double DistributedQuantity { get; set; }
        }

        public async Task<int> CreateAsync(MaterialsRequestNote model)
        {
            int created = 0;
            using (var transaction = _inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    List<string> productionOrderIds = new List<string>();
                    model = await CustomCodeGenerator(model);
                    model.FlagForCreate(_identityInterface.Username, UserAgent);
                    model.FlagForUpdate(_identityInterface.Username, UserAgent);
                    foreach (var item in model.MaterialsRequestNoteItems)
                    {
                        item.FlagForCreate(_identityInterface.Username, UserAgent);
                        item.FlagForUpdate(_identityInterface.Username, UserAgent);
                        productionOrderIds.Add(item.ProductionOrderId);
                    }
                    _dbSet.Add(model);

                    created = await _inventoryDbContext.SaveChangesAsync();

                    UpdateIsRequestedProductionOrder(productionOrderIds, "CREATE");
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return created;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int deleted = 0;
            using (var transaction = this._inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    List<string> productionOrderIds = new List<string>();
                    MaterialsRequestNote Model = await ReadByIdAsync(id);
                    if (Model == null)
                        throw new Exception("data not found");
                    Model.FlagForDelete(_identityInterface.Username, UserAgent);

                    deleted = await _inventoryDbContext.SaveChangesAsync();

                    foreach (var item in Model.MaterialsRequestNoteItems)
                    {
                        await DeleteItem(item);
                        productionOrderIds.Add(item.ProductionOrderId);
                    }

                    UpdateIsRequestedProductionOrder(productionOrderIds, "DELETE");
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return deleted;
        }

        public MaterialsRequestNote MapToModel(MaterialsRequestNoteViewModel viewModel)
        {
            MaterialsRequestNote model = new MaterialsRequestNote();

            PropertyCopier<MaterialsRequestNoteViewModel, MaterialsRequestNote>.Copy(viewModel, model);

            model.UnitId = viewModel.Unit.Id;
            model.UnitCode = viewModel.Unit.Code;
            model.UnitName = viewModel.Unit.Name;
            model.RequestType = viewModel.RequestType;
            model.Remark = viewModel.Remark;

            model.MaterialsRequestNoteItems = new List<MaterialsRequestNoteItem>();

            foreach (MaterialsRequestNoteItemViewModel materialsRequestNoteItemViewModel in viewModel.MaterialsRequestNoteItems)
            {
                MaterialsRequestNoteItem materialsRequestNoteItem = new MaterialsRequestNoteItem();

                PropertyCopier<MaterialsRequestNoteItemViewModel, MaterialsRequestNoteItem>.Copy(materialsRequestNoteItemViewModel, materialsRequestNoteItem);

                if (!viewModel.RequestType.Equals("PURCHASE") && !viewModel.RequestType.Equals("TEST"))
                {

                    materialsRequestNoteItem.ProductionOrderId = materialsRequestNoteItemViewModel.ProductionOrder.Id;
                    materialsRequestNoteItem.ProductionOrderNo = materialsRequestNoteItemViewModel.ProductionOrder.OrderNo;
                    materialsRequestNoteItem.ProductionOrderIsCompleted = materialsRequestNoteItemViewModel.ProductionOrder.IsCompleted;
                    materialsRequestNoteItem.OrderQuantity = materialsRequestNoteItemViewModel.ProductionOrder.OrderQuantity.GetValueOrDefault();
                    materialsRequestNoteItem.OrderTypeId = materialsRequestNoteItemViewModel.ProductionOrder.OrderType.Id;
                    materialsRequestNoteItem.OrderTypeCode = materialsRequestNoteItemViewModel.ProductionOrder.OrderType.Code;
                    materialsRequestNoteItem.OrderTypeName = materialsRequestNoteItemViewModel.ProductionOrder.OrderType.Name;
                }

                materialsRequestNoteItem.ProductId = materialsRequestNoteItemViewModel.Product.Id;
                materialsRequestNoteItem.ProductCode = materialsRequestNoteItemViewModel.Product.Code;
                materialsRequestNoteItem.ProductName = materialsRequestNoteItemViewModel.Product.Name;
                materialsRequestNoteItem.Length = materialsRequestNoteItemViewModel.Length != null ? (double)materialsRequestNoteItemViewModel.Length : 0;
                materialsRequestNoteItem.DistributedLength = materialsRequestNoteItemViewModel.DistributedLength != null ? (double)materialsRequestNoteItemViewModel.DistributedLength : 0;

                model.MaterialsRequestNoteItems.Add(materialsRequestNoteItem);
            }

            return model;
        }

        public MaterialsRequestNoteViewModel MapToViewModel(MaterialsRequestNote model)
        {
            MaterialsRequestNoteViewModel viewModel = new MaterialsRequestNoteViewModel();

            PropertyCopier<MaterialsRequestNote, MaterialsRequestNoteViewModel>.Copy(model, viewModel);

            CodeNameViewModel Unit = new CodeNameViewModel()
            {
                Id = model.UnitId,
                Code = model.UnitCode,
                Name = model.UnitName
            };

            viewModel.Code = model.Code;
            viewModel.Unit = Unit;
            viewModel.RequestType = model.RequestType;
            viewModel.Remark = model.Remark;

            viewModel.MaterialsRequestNoteItems = new List<MaterialsRequestNoteItemViewModel>();
            if (model.MaterialsRequestNoteItems != null)
            {
                foreach (MaterialsRequestNoteItem materialsRequestNoteItem in model.MaterialsRequestNoteItems)
                {
                    MaterialsRequestNoteItemViewModel materialsRequestNoteItemViewModel = new MaterialsRequestNoteItemViewModel();
                    PropertyCopier<MaterialsRequestNoteItem, MaterialsRequestNoteItemViewModel>.Copy(materialsRequestNoteItem, materialsRequestNoteItemViewModel);

                    CodeNameViewModel OrderType = new CodeNameViewModel()
                    {
                        Id = materialsRequestNoteItem.OrderTypeId,
                        Code = materialsRequestNoteItem.OrderTypeCode,
                        Name = materialsRequestNoteItem.OrderTypeName
                    };

                    ProductionOrderViewModel ProductionOrder = new ProductionOrderViewModel()
                    {
                        Id = materialsRequestNoteItem.ProductionOrderId,
                        OrderNo = materialsRequestNoteItem.ProductionOrderNo,
                        OrderQuantity = materialsRequestNoteItem.OrderQuantity,
                        IsCompleted = materialsRequestNoteItem.ProductionOrderIsCompleted,
                        DistributedQuantity = materialsRequestNoteItem.DistributedLength,
                        OrderType = OrderType
                    };
                    materialsRequestNoteItemViewModel.ProductionOrder = ProductionOrder;

                    CodeNameViewModel Product = new CodeNameViewModel()
                    {
                        Id = materialsRequestNoteItem.ProductId,
                        Code = materialsRequestNoteItem.ProductCode,
                        Name = materialsRequestNoteItem.ProductName
                    };
                    materialsRequestNoteItemViewModel.Product = Product;

                    materialsRequestNoteItemViewModel.Length = materialsRequestNoteItem.Length;
                    materialsRequestNoteItemViewModel.DistributedLength = materialsRequestNoteItem.DistributedLength;

                    viewModel.MaterialsRequestNoteItems.Add(materialsRequestNoteItemViewModel);
                }
            }

            return viewModel;
        }

        public ReadResponse<MaterialsRequestNote> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<MaterialsRequestNote> query = this._inventoryDbContext.MaterialsRequestNotes;

            List<string> searchAttributes = new List<string>()
                {
                    "UnitName", "RequestType", "Code", "MaterialsRequestNoteItems.ProductionOrderNo"
                };
            query = QueryHelper<MaterialsRequestNote>.Search(query, searchAttributes, keyword);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "Code", "Unit", "RequestType", "Remark", "MaterialsRequestNoteItems", "LastModifiedUtc", "IsCompleted"
                };
            query = query
                .Select(mrn => new MaterialsRequestNote
                {
                    Id = mrn.Id,
                    Code = mrn.Code,
                    UnitId = mrn.UnitId,
                    UnitCode = mrn.UnitCode,
                    UnitName = mrn.UnitName,
                    IsDistributed = mrn.IsDistributed,
                    IsCompleted = mrn.IsCompleted,
                    RequestType = mrn.RequestType,
                    LastModifiedUtc = mrn.LastModifiedUtc,
                    MaterialsRequestNoteItems = mrn.MaterialsRequestNoteItems.Select(p => new MaterialsRequestNoteItem { MaterialsRequestNoteId = p.MaterialsRequestNoteId, ProductionOrderNo = p.ProductionOrderNo }).Where(i => i.MaterialsRequestNoteId.Equals(mrn.Id)).ToList()
                });

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<MaterialsRequestNote>.Filter(query, filterDictionary);

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<MaterialsRequestNote>.Order(query, orderDictionary);

            Pageable<MaterialsRequestNote> pageable = new Pageable<MaterialsRequestNote>(query, page - 1, size);
            List<MaterialsRequestNote> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return new ReadResponse<MaterialsRequestNote>(data, totalData, orderDictionary, selectedFields);
        }

        public async Task<MaterialsRequestNote> ReadByIdAsync(int id)
        {
            return await this._dbSet
                .Include(d => d.MaterialsRequestNoteItems)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, MaterialsRequestNote model)
        {
            int updated = 0;
            var internalTransaction = _inventoryDbContext.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? _inventoryDbContext.Database.CurrentTransaction : _inventoryDbContext.Database.BeginTransaction();

            try
            {
                var dbModel = await ReadByIdAsync(id);

                if (dbModel == null)
                    throw new Exception("data not found");

                dbModel.Remark = model.Remark;
                dbModel.IsCompleted = model.IsCompleted;
                dbModel.IsDistributed = model.IsDistributed;
                dbModel.RequestType = model.RequestType;
                dbModel.Type = model.Type;
                dbModel.UnitCode = model.UnitCode;
                dbModel.UnitId = model.UnitId;
                dbModel.UnitName = model.UnitName;
                dbModel.FlagForUpdate(_identityInterface.Username, UserAgent);
                updated = await _inventoryDbContext.SaveChangesAsync();

                var deletedDetails = dbModel.MaterialsRequestNoteItems.Where(x => !model.MaterialsRequestNoteItems.Any(y => x.Id == y.Id));
                var updatedDetails = dbModel.MaterialsRequestNoteItems.Where(x => model.MaterialsRequestNoteItems.Any(y => x.Id == y.Id));
                var addedDetails = model.MaterialsRequestNoteItems.Where(x => !dbModel.MaterialsRequestNoteItems.Any(y => y.Id == x.Id));
                List<string> deletedProductionOrderIds = new List<string>();
                List<string> newProductionOrderIds = new List<string>();
                foreach (var item in deletedDetails)
                {
                    updated += await DeleteItem(item);
                    deletedProductionOrderIds.Add(item.ProductionOrderId);
                }

                foreach (var item in updatedDetails)
                {
                    var selectedDetail = model.MaterialsRequestNoteItems.FirstOrDefault(x => x.Id == item.Id);

                    if (item.ProductionOrderId != selectedDetail.ProductionOrderId)
                    {
                        newProductionOrderIds.Add(selectedDetail.ProductionOrderId);
                        deletedProductionOrderIds.Add(item.ProductionOrderId);
                    }

                    item.ProductionOrderId = selectedDetail.ProductionOrderId;
                    item.ProductionOrderIsCompleted = selectedDetail.ProductionOrderIsCompleted;
                    item.ProductionOrderNo = selectedDetail.ProductionOrderNo;
                    item.ProductId = selectedDetail.ProductId;
                    item.ProductCode = selectedDetail.ProductCode;
                    item.ProductName = selectedDetail.ProductName;
                    item.Grade = selectedDetail.Grade;
                    item.Length = selectedDetail.Length;
                    item.Remark = selectedDetail.Remark;
                    item.DistributedLength = selectedDetail.DistributedLength;
                    item.OrderQuantity = selectedDetail.OrderQuantity;
                    item.OrderTypeCode = selectedDetail.OrderTypeCode;
                    item.OrderTypeId = selectedDetail.OrderTypeId;
                    item.OrderTypeName = selectedDetail.OrderTypeName;

                    updated += await UpdateItem(item);
                }

                foreach (var item in addedDetails)
                {
                    item.MaterialsRequestNoteId = id;
                    updated += await CreateItem(item);
                    newProductionOrderIds.Add(item.ProductionOrderId);
                }
                UpdateIsRequestedProductionOrder(deletedProductionOrderIds, "DELETE");
                UpdateIsRequestedProductionOrder(newProductionOrderIds, "CREATE");

                if (internalTransaction)
                    transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }

            return updated;
        }

        public async Task<MaterialsRequestNote> CustomCodeGenerator(MaterialsRequestNote model)
        {
            model.Type = string.Equals(model.UnitName.ToUpper(), "PRINTING") ? "P" : "F";
            var lastData = await _dbSet.Where(w => w.UnitCode == model.UnitCode).OrderByDescending(o => o.CreatedUtc).FirstOrDefaultAsync();

            DateTime Now = DateTime.Now;
            string year = Now.ToString("yy");
            string month = Now.ToString("MM");

            if (lastData == null)
            {
                model.AutoIncrementNumber = 1;
                string number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                model.Code = $"SPB{model.Type}{month}{year}{number}";
            }
            else
            {
                if (lastData.CreatedUtc.Year < Now.Year)
                {
                    model.AutoIncrementNumber = 1;
                    string number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                    model.Code = $"SPB{model.Type}{month}{year}{number}";
                }
                else
                {
                    model.AutoIncrementNumber = lastData.AutoIncrementNumber + 1;
                    string number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                    model.Code = $"SPB{model.Type}{month}{year}{number}";
                }
            }

            return model;
        }

        public void UpdateIsRequestedProductionOrder(List<string> productionOrderIds, string context)
        {
            string productionOrderUri;
            if (context == "DELETE")
            {
                productionOrderUri = "sales/production-orders/update-requested-false";
            }
            else
            {
                productionOrderUri = "sales/production-orders/update-requested-true";
            }

            _ = new
            {
                context,
                ids = productionOrderIds
            };

            IHttpServiceRepository httpClient = (IHttpServiceRepository)_serviceProvider.GetService(typeof(IHttpServiceRepository));

            var response = httpClient.PutAsync($"{APIEndpoint.Sales}{productionOrderUri}", new StringContent(JsonConvert.SerializeObject(productionOrderIds).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
            response.EnsureSuccessStatusCode();
        }

        public void UpdateIsCompletedProductionOrder(string productionOrderId)
        {
            string productionOrderUri = "sales/production-orders/update-iscompleted-true";

            IHttpServiceRepository httpClient = (IHttpServiceRepository)this._serviceProvider.GetService(typeof(IHttpServiceRepository));

            var response = httpClient.PutAsync($"{APIEndpoint.Sales}{productionOrderUri}", new StringContent(productionOrderId, Encoding.UTF8, General.JsonMediaType)).Result;
            response.EnsureSuccessStatusCode();
        }

        public void UpdateDistributedQuantityProductionOrder(List<SppParams> contextAndIds)
        {
            string productionOrderUri = "sales/production-orders/update-distributed-quantity";
            _ = new
            {
                data = contextAndIds
            };

            IHttpServiceRepository httpClient = (IHttpServiceRepository)this._serviceProvider.GetService(typeof(IHttpServiceRepository));
            var response = httpClient.PutAsync($"{APIEndpoint.Sales}{productionOrderUri}", new StringContent(JsonConvert.SerializeObject(contextAndIds).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIsCompleted(int Id, MaterialsRequestNote model)
        {
            try
            {
                int countIsIncomplete = 0;
                List<SppParams> contextAndIds = new List<SppParams>();
                foreach (MaterialsRequestNoteItem item in model.MaterialsRequestNoteItems)
                {
                    SppParams sppParams = new SppParams();
                    if (!item.ProductionOrderIsCompleted)
                    {
                        countIsIncomplete += 1;
                        sppParams.Context = "INCOMPLETE";
                        sppParams.Id = item.ProductionOrderId;
                    }
                    else
                    {
                        sppParams.Context = "COMPLETE";
                        sppParams.Id = item.ProductionOrderId;
                    }

                    contextAndIds.Add(sppParams);
                    UpdateIsCompletedProductionOrder(item.ProductionOrderId);
                }

                if (countIsIncomplete == 0)
                {
                    model.IsCompleted = true;
                }
                else
                {
                    model.IsCompleted = false;
                }

                await UpdateAsync(Id, model);
            }
            catch (Exception e)
            {
                throw e;
            };
        }

        public void UpdateDistributedQuantity(int Id, MaterialsRequestNote Model)
        {
            {
                try
                {
                    List<SppParams> contextQuantityAndIds = new List<SppParams>();
                    foreach (MaterialsRequestNoteItem item in Model.MaterialsRequestNoteItems)
                    {
                        SppParams sppParams = new SppParams
                        {
                            Id = item.ProductionOrderId,
                            DistributedQuantity = item.DistributedLength
                        };

                        contextQuantityAndIds.Add(sppParams);
                    }
                    UpdateDistributedQuantityProductionOrder(contextQuantityAndIds);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private async Task<int> DeleteItem(MaterialsRequestNoteItem item)
        {
            item.FlagForDelete(_identityInterface.Username, UserAgent);
            return await _inventoryDbContext.SaveChangesAsync();
        }

        private async Task<int> CreateItem(MaterialsRequestNoteItem item)
        {
            item.FlagForCreate(_identityInterface.Username, UserAgent);
            item.FlagForUpdate(_identityInterface.Username, UserAgent);
            _inventoryDbContext.MaterialsRequestNoteItems.Add(item);
            return await _inventoryDbContext.SaveChangesAsync();
        }

        private async Task<int> UpdateItem(MaterialsRequestNoteItem item)
        {
            item.FlagForUpdate(_identityInterface.Username, UserAgent);
            return await _inventoryDbContext.SaveChangesAsync();
        }

        public IQueryable<MaterialsRequestNoteReportViewModel> GetReportQuery(string materialsRequestNoteCode, string productionOrderId, string unitId, string productId, string status, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            bool isCompleted = !string.IsNullOrWhiteSpace(status) && status.ToUpper().Equals("ALREADY COMPLETE") ? true : false;
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var query = (from a in _inventoryDbContext.MaterialsRequestNotes
                         join b in _inventoryDbContext.MaterialsRequestNoteItems on a.Id equals b.MaterialsRequestNoteId
                         where a.IsDeleted == false
                             && a.Code == (string.IsNullOrWhiteSpace(materialsRequestNoteCode) ? a.Code : materialsRequestNoteCode)
                             && a.UnitId == (string.IsNullOrWhiteSpace(unitId) ? a.UnitId : unitId)
                             && b.ProductionOrderId == (string.IsNullOrWhiteSpace(productionOrderId) ? b.ProductionOrderId : productionOrderId)
                             && b.ProductId == (string.IsNullOrWhiteSpace(productId) ? b.ProductId : productId)
                             && b.ProductionOrderIsCompleted == (string.IsNullOrWhiteSpace(status) ? b.ProductionOrderIsCompleted : isCompleted)
                             && a.CreatedUtc.AddHours(offset).Date >= DateFrom.Date
                             && a.CreatedUtc.AddHours(offset).Date <= DateTo.Date
                         select new MaterialsRequestNoteReportViewModel
                         {
                             Code = a.Code,
                             CreatedDate = a.CreatedUtc,
                             OrderNo = b.ProductionOrderNo,
                             ProductName = b.ProductName,
                             Grade = b.Grade,
                             OrderQuantity = b.OrderQuantity,
                             Length = b.Length,
                             DistributedLength = b.DistributedLength,
                             Status = b.ProductionOrderIsCompleted,
                             Remark = a.Remark,
                         });

            return query;
        }

        public Tuple<List<MaterialsRequestNoteReportViewModel>, int> GetReport(string materialsRequestNoteCode, string productionOrderId, string unitId, string productId, string status, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset)
        {
            var query = GetReportQuery(materialsRequestNoteCode, productionOrderId, unitId, productId, status, dateFrom, dateTo, offset);

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            if (orderDictionary.Count.Equals(0))
            {
                query = query.OrderByDescending(b => b.CreatedDate);
            }
            else
            {
                string key = orderDictionary.Keys.First();
                string orderType = orderDictionary[key];

                query = query.OrderBy(string.Concat(key, " ", orderType));
            }

            Pageable<MaterialsRequestNoteReportViewModel> pageable = new Pageable<MaterialsRequestNoteReportViewModel>(query, page - 1, size);
            List<MaterialsRequestNoteReportViewModel> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return Tuple.Create(data, totalData);
        }
    }
}
