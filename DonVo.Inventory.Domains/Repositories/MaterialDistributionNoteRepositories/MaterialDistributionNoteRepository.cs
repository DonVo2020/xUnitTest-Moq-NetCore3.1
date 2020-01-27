using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories;
using DonVo.Inventory.Infrastructures.Helpers;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels;
using DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories.MaterialDistributionNoteRepositories
{
    public class MaterialDistributionNoteRepository: IMaterialDistributionNoteRepository
    {
        private const string UserAgent = "inventory-service";
        protected DbSet<MaterialDistributionNote> _dbSet;
        public IIdentityInterface _identityInterface;
        public readonly IServiceProvider _serviceProvider;
        public InventoryDbContext _inventoryDbContext;

        public MaterialDistributionNoteRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            _inventoryDbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dbSet = dbContext.Set<MaterialDistributionNote>();
            _identityInterface = serviceProvider.GetService<IIdentityInterface>();
        }

        public async Task<int> CreateAsync(MaterialDistributionNote model)
        {
            int created = 0;
            using (var transaction = _inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    model = await this.CustomCodeGenerator(model);
                    model.FlagForCreate(_identityInterface.Username, UserAgent);
                    model.FlagForUpdate(_identityInterface.Username, UserAgent);
                    foreach (var item in model.MaterialDistributionNoteItems)
                    {
                        item.FlagForCreate(_identityInterface.Username, UserAgent);
                        item.FlagForUpdate(_identityInterface.Username, UserAgent);
                        foreach (var detail in item.MaterialDistributionNoteDetails)
                        {
                            detail.FlagForCreate(_identityInterface.Username, UserAgent);
                            detail.FlagForUpdate(_identityInterface.Username, UserAgent);
                        }
                    }

                    _dbSet.Add(model);

                    created = await _inventoryDbContext.SaveChangesAsync();

                    IMaterialRequestNoteRepository materialsRequestNoteService = _serviceProvider.GetService<IMaterialRequestNoteRepository>();

                    List<ViewModels.InventoryViewModel.InventorySummaryViewModel> data = new List<ViewModels.InventoryViewModel.InventorySummaryViewModel>();


                    foreach (MaterialDistributionNoteItem materialDistributionNoteItem in model.MaterialDistributionNoteItems)
                    {
                        MaterialsRequestNote materialsRequestNote = await materialsRequestNoteService.ReadByIdAsync(materialDistributionNoteItem.MaterialRequestNoteId);
                        materialsRequestNote.IsDistributed = true;

                        if (model.Type.ToUpper().Equals("PRODUCTION"))
                        {
                            foreach (MaterialDistributionNoteDetail materialDistributionNoteDetail in materialDistributionNoteItem.MaterialDistributionNoteDetails)
                            {
                                materialsRequestNote.MaterialsRequestNoteItems.Where(w => w.ProductionOrderId.Equals(materialDistributionNoteDetail.ProductionOrderId)).Select(s => { s.DistributedLength += materialDistributionNoteDetail.ReceivedLength; return s; }).ToList();
                            }
                            materialsRequestNoteService.UpdateDistributedQuantity(materialsRequestNote.Id, materialsRequestNote);

                        }
                        await materialsRequestNoteService.UpdateAsync(materialsRequestNote.Id, materialsRequestNote);

                    }

                    _inventoryDbContext.SaveChanges();

                    await CreateInventoryDocument(model, "OUT");

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
            int count = 0;

            using (var trans = _inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    MaterialDistributionNote materialDistributionNote = await ReadByIdAsync(id);
                    materialDistributionNote.FlagForDelete(_identityInterface.Username, UserAgent);
                    foreach (var item in materialDistributionNote.MaterialDistributionNoteItems)
                    {
                        item.FlagForDelete(_identityInterface.Username, UserAgent);

                        foreach (var detail in item.MaterialDistributionNoteDetails)
                        {
                            detail.FlagForDelete(_identityInterface.Username, UserAgent);
                        }
                    }

                    count = await _inventoryDbContext.SaveChangesAsync();

                    IMaterialRequestNoteRepository materialsRequestNoteService = _serviceProvider.GetService<IMaterialRequestNoteRepository>();

                    foreach (MaterialDistributionNoteItem materialDistributionNoteItem in materialDistributionNote.MaterialDistributionNoteItems)
                    {
                        MaterialsRequestNote materialsRequestNote = await materialsRequestNoteService.ReadByIdAsync(materialDistributionNoteItem.MaterialRequestNoteId);
                        materialsRequestNote.IsDistributed = true;

                        if (materialDistributionNote.Type.ToUpper().Equals("PRODUCTION"))
                        {
                            foreach (MaterialDistributionNoteDetail materialDistributionNoteDetail in materialDistributionNoteItem.MaterialDistributionNoteDetails)
                            {
                                materialsRequestNote.MaterialsRequestNoteItems
                                    .Where(w => w.ProductionOrderId
                                    .Equals(materialDistributionNoteDetail.ProductionOrderId))
                                    .Select(s => { s.DistributedLength -= materialDistributionNoteDetail.ReceivedLength; return s; }).ToList();
                            }
                            materialsRequestNoteService.UpdateDistributedQuantity(materialsRequestNote.Id, materialsRequestNote);
                        }
                        await materialsRequestNoteService.UpdateAsync(materialsRequestNote.Id, materialsRequestNote);
                    }

                    await CreateInventoryDocument(materialDistributionNote, "IN");
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return count;
        }

        public List<MaterialDistributionNoteReportViewModel> GetPdfReport(string unitId, string unitName, string type, DateTime date, int offset)
        {
            var data = GetReportQuery(unitId, type, date, offset).ToList();

            return data;
        }

        public Tuple<List<MaterialDistributionNoteReportViewModel>, int> GetReport(string unitId, string type, DateTime date, int page, int size, string order, int offset)
        {
            var query = GetReportQuery(unitId, type, date, offset);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            if (OrderDictionary.Count.Equals(0))
            {
                query = query.OrderByDescending(b => b.LastModifiedUtc);
            }
            else
            {
                string key = OrderDictionary.Keys.First();
                string orderType = OrderDictionary[key];

                query = query.OrderBy(string.Concat(key, " ", orderType));
            }

            Pageable<MaterialDistributionNoteReportViewModel> pageable = new Pageable<MaterialDistributionNoteReportViewModel>(query, page - 1, size);
            List<MaterialDistributionNoteReportViewModel> Data = pageable.Data.ToList<MaterialDistributionNoteReportViewModel>();
            int totalData = pageable.TotalCount;

            return Tuple.Create(Data, totalData);
        }

        public MaterialDistributionNote MapToModel(MaterialDistributionNoteViewModel viewModel)
        {
            MaterialDistributionNote model = new MaterialDistributionNote();
            PropertyCopier<MaterialDistributionNoteViewModel, MaterialDistributionNote>.Copy(viewModel, model);

            model.UnitId = viewModel.Unit.Id.ToString();
            model.UnitCode = viewModel.Unit.Code;
            model.UnitName = viewModel.Unit.Name;

            model.MaterialDistributionNoteItems = new List<MaterialDistributionNoteItem>();
            foreach (MaterialDistributionNoteItemViewModel mdni in viewModel.MaterialDistributionNoteItems)
            {
                MaterialDistributionNoteItem materialDistributionNoteItem = new MaterialDistributionNoteItem();
                PropertyCopier<MaterialDistributionNoteItemViewModel, MaterialDistributionNoteItem>.Copy(mdni, materialDistributionNoteItem);

                materialDistributionNoteItem.MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetail>();
                foreach (MaterialDistributionNoteDetailViewModel mdnd in mdni.MaterialDistributionNoteDetails)
                {
                    MaterialDistributionNoteDetail materialDistributionNoteDetail = new MaterialDistributionNoteDetail();
                    PropertyCopier<MaterialDistributionNoteDetailViewModel, MaterialDistributionNoteDetail>.Copy(mdnd, materialDistributionNoteDetail);

                    materialDistributionNoteDetail.ProductionOrderId = mdnd.ProductionOrder.Id;
                    materialDistributionNoteDetail.ProductionOrderNo = mdnd.ProductionOrder.OrderNo;
                    materialDistributionNoteDetail.ProductionOrderIsCompleted = mdnd.ProductionOrder.IsCompleted;
                    materialDistributionNoteDetail.DistributedLength = mdnd.DistributedLength.GetValueOrDefault();

                    materialDistributionNoteDetail.ProductId = mdnd.Product.Id;
                    materialDistributionNoteDetail.ProductCode = mdnd.Product.Code;
                    materialDistributionNoteDetail.ProductName = mdnd.Product.Name;

                    materialDistributionNoteDetail.SupplierId = mdnd.Supplier.Id;
                    materialDistributionNoteDetail.SupplierCode = mdnd.Supplier.Code;
                    materialDistributionNoteDetail.SupplierName = mdnd.Supplier.Name;

                    materialDistributionNoteItem.MaterialDistributionNoteDetails.Add(materialDistributionNoteDetail);
                }

                model.MaterialDistributionNoteItems.Add(materialDistributionNoteItem);
            }

            return model;
        }

        public MaterialDistributionNoteViewModel MapToViewModel(MaterialDistributionNote model)
        {
            MaterialDistributionNoteViewModel viewModel = new MaterialDistributionNoteViewModel();
            PropertyCopier<MaterialDistributionNote, MaterialDistributionNoteViewModel>.Copy(model, viewModel);

            CodeNameViewModel Unit = new CodeNameViewModel()
            {
                Id = model.UnitId,
                Code = model.UnitCode,
                Name = model.UnitName
            };

            viewModel.Unit = Unit;

            viewModel.MaterialDistributionNoteItems = new List<MaterialDistributionNoteItemViewModel>();
            if (model.MaterialDistributionNoteItems != null)
            {
                foreach (MaterialDistributionNoteItem mdni in model.MaterialDistributionNoteItems)
                {
                    MaterialDistributionNoteItemViewModel materialDistributionNoteItemViewModel = new MaterialDistributionNoteItemViewModel();
                    PropertyCopier<MaterialDistributionNoteItem, MaterialDistributionNoteItemViewModel>.Copy(mdni, materialDistributionNoteItemViewModel);

                    materialDistributionNoteItemViewModel.MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetailViewModel>();
                    foreach (MaterialDistributionNoteDetail mdnd in mdni.MaterialDistributionNoteDetails)
                    {
                        MaterialDistributionNoteDetailViewModel materialDistributionNoteDetailViewModel = new MaterialDistributionNoteDetailViewModel();
                        PropertyCopier<MaterialDistributionNoteDetail, MaterialDistributionNoteDetailViewModel>.Copy(mdnd, materialDistributionNoteDetailViewModel);

                        ProductionOrderViewModel productionOrder = new ProductionOrderViewModel
                        {
                            Id = mdnd.ProductionOrderId,
                            OrderNo = mdnd.ProductionOrderNo,
                            IsCompleted = mdnd.ProductionOrderIsCompleted,
                        };

                        CodeNameViewModel product = new CodeNameViewModel
                        {
                            Id = mdnd.ProductId,
                            Code = mdnd.ProductCode,
                            Name = mdnd.ProductName
                        };

                        CodeNameViewModel supplier = new CodeNameViewModel
                        {
                            Id = mdnd.SupplierId,
                            Code = mdnd.SupplierCode,
                            Name = mdnd.SupplierName
                        };

                        materialDistributionNoteDetailViewModel.ProductionOrder = productionOrder;
                        materialDistributionNoteDetailViewModel.Product = product;
                        materialDistributionNoteDetailViewModel.Supplier = supplier;

                        materialDistributionNoteItemViewModel.MaterialDistributionNoteDetails.Add(materialDistributionNoteDetailViewModel);
                    }

                    viewModel.MaterialDistributionNoteItems.Add(materialDistributionNoteItemViewModel);
                }
            }

            return viewModel;
        }

        public ReadResponse<MaterialDistributionNote> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<MaterialDistributionNote> query = _inventoryDbContext.MaterialDistributionNotes;

            List<string> searchAttributes = new List<string>()
            {
                "No"
            };

            query = QueryHelper<MaterialDistributionNote>.Search(query, searchAttributes, keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "No", "createdUtc", "Type", "IsDisposition", "IsApproved"
            };

            query = query
                .Select(mdn => new MaterialDistributionNote
                {
                    Id = mdn.Id,
                    No = mdn.No,
                    CreatedUtc = mdn.CreatedUtc,
                    Type = mdn.Type,
                    IsDisposition = mdn.IsDisposition,
                    IsApproved = mdn.IsApproved,
                    LastModifiedUtc = mdn.LastModifiedUtc
                });

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<MaterialDistributionNote>.Filter(query, filterDictionary);

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<MaterialDistributionNote>.Order(query, orderDictionary);

            Pageable<MaterialDistributionNote> pageable = new Pageable<MaterialDistributionNote>(query, page - 1, size);
            List<MaterialDistributionNote> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return new ReadResponse<MaterialDistributionNote>(data, totalData, orderDictionary, SelectedFields);
        }

        public async Task<MaterialDistributionNote> ReadByIdAsync(int id)
        {
            var data = await _dbSet
                .Where(d => d.Id.Equals(id) && d.IsDeleted.Equals(false))
                .Include(d => d.MaterialDistributionNoteItems)
                    .ThenInclude(d => d.MaterialDistributionNoteDetails)
                .FirstOrDefaultAsync();

            if (data != null)
            {
                List<int> detailsId = new List<int>();

                foreach (MaterialDistributionNoteItem item in data.MaterialDistributionNoteItems)
                {
                    foreach (MaterialDistributionNoteDetail detail in item.MaterialDistributionNoteDetails)
                    {
                        detailsId.Add(detail.MaterialsRequestNoteItemId);
                    }
                }

                var requestNoteItems = _inventoryDbContext.MaterialsRequestNoteItems.Select(p => new { p.Id, IsCompleted = p.ProductionOrderIsCompleted }).Where(p => detailsId.Contains(p.Id));

                foreach (MaterialDistributionNoteItem item in data.MaterialDistributionNoteItems)
                {
                    foreach (MaterialDistributionNoteDetail detail in item.MaterialDistributionNoteDetails)
                    {
                        var requestNoteItem = requestNoteItems.FirstOrDefault(p => p.Id == detail.MaterialsRequestNoteItemId);
                        if (requestNoteItem != null)
                            detail.IsCompleted = requestNoteItem.IsCompleted;
                    }
                }
            }

            return data;
        }

        public async Task<int> UpdateAsync(int id, MaterialDistributionNote model)
        {
            int updated = 0;
            using (var transaction = _inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (id != model.Id)
                        throw new Exception("data not found");

                    var dbModel = await ReadByIdAsync(id);

                    dbModel.IsApproved = model.IsApproved;
                    dbModel.IsDisposition = model.IsDisposition;
                    dbModel.AutoIncrementNumber = model.AutoIncrementNumber;
                    dbModel.No = model.No;
                    dbModel.Type = model.Type;
                    dbModel.UnitCode = model.UnitCode;
                    dbModel.UnitId = model.UnitId;
                    dbModel.UnitName = model.UnitName;
                    dbModel.FlagForUpdate(_identityInterface.Username, UserAgent);

                    var deletedItems = dbModel.MaterialDistributionNoteItems.Where(x => !model.MaterialDistributionNoteItems.Any(y => x.Id == y.Id));
                    var updatedItems = dbModel.MaterialDistributionNoteItems.Where(x => model.MaterialDistributionNoteItems.Any(y => x.Id == y.Id));
                    var addedItems = model.MaterialDistributionNoteItems.Where(x => !dbModel.MaterialDistributionNoteItems.Any(y => y.Id == x.Id));

                    foreach (var item in deletedItems)
                    {
                        item.FlagForDelete(_identityInterface.Username, UserAgent);
                        foreach (var detail in item.MaterialDistributionNoteDetails)
                        {
                            detail.FlagForDelete(_identityInterface.Username, UserAgent);
                        }
                    }

                    foreach (var item in updatedItems)
                    {
                        var selectedItem = model.MaterialDistributionNoteItems.FirstOrDefault(x => x.Id == item.Id);

                        item.MaterialRequestNoteCode = selectedItem.MaterialRequestNoteCode;
                        item.MaterialRequestNoteCreatedDateUtc = selectedItem.MaterialRequestNoteCreatedDateUtc;
                        item.MaterialRequestNoteId = selectedItem.MaterialRequestNoteId;
                        item.FlagForUpdate(_identityInterface.Username, UserAgent);

                        var deletedDetails = item.MaterialDistributionNoteDetails.Where(x => !selectedItem.MaterialDistributionNoteDetails.Any(y => x.Id == y.Id));
                        var updatedDetails = item.MaterialDistributionNoteDetails.Where(x => selectedItem.MaterialDistributionNoteDetails.Any(y => x.Id == y.Id));
                        var addedDetails = selectedItem.MaterialDistributionNoteDetails.Where(x => !item.MaterialDistributionNoteDetails.Any(y => y.Id == x.Id));

                        foreach (var detail in deletedDetails)
                        {
                            item.FlagForDelete(_identityInterface.Username, UserAgent);
                        }

                        foreach (var detail in updatedDetails)
                        {
                            var selectedDetail = selectedItem.MaterialDistributionNoteDetails.FirstOrDefault(x => x.Id == detail.Id);

                            detail.DistributedLength = selectedDetail.DistributedLength;
                            detail.Grade = selectedDetail.Grade;
                            detail.IsCompleted = selectedDetail.IsCompleted;
                            detail.IsDisposition = selectedDetail.IsDisposition;
                            detail.MaterialRequestNoteItemLength = selectedDetail.MaterialRequestNoteItemLength;
                            detail.MaterialsRequestNoteItemId = selectedDetail.MaterialsRequestNoteItemId;
                            detail.ProductCode = selectedDetail.ProductCode;
                            detail.ProductId = selectedDetail.ProductId;
                            detail.ProductionOrderId = selectedDetail.ProductionOrderId;
                            detail.ProductionOrderIsCompleted = selectedDetail.ProductionOrderIsCompleted;
                            detail.ProductionOrderNo = selectedDetail.ProductionOrderNo;
                            detail.ProductName = selectedDetail.ProductName;
                            detail.Quantity = selectedDetail.Quantity;
                            detail.ReceivedLength = selectedDetail.ReceivedLength;
                            detail.SupplierCode = selectedDetail.SupplierCode;
                            detail.SupplierId = selectedDetail.SupplierId;
                            detail.SupplierName = selectedDetail.SupplierName;
                            detail.FlagForUpdate(_identityInterface.Username, UserAgent);
                        }

                        foreach (var detail in addedDetails)
                        {
                            detail.MaterialDistributionNoteItemId = id;
                            detail.FlagForCreate(_identityInterface.Username, UserAgent);
                            detail.FlagForUpdate(_identityInterface.Username, UserAgent);
                        }
                    }

                    foreach (var item in addedItems)
                    {
                        item.MaterialDistributionNoteId = id;
                        item.FlagForCreate(_identityInterface.Username, UserAgent);
                        item.FlagForUpdate(_identityInterface.Username, UserAgent);
                        foreach (var detail in item.MaterialDistributionNoteDetails)
                        {
                            detail.FlagForCreate(_identityInterface.Username, UserAgent);
                            detail.FlagForUpdate(_identityInterface.Username, UserAgent);
                        }
                    }
                    updated = await _inventoryDbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return updated;
        }

        public bool UpdateIsApprove(List<int> Ids)
        {
            bool isSuccessful = false;

            using (var Transaction = _inventoryDbContext.Database.BeginTransaction())
            {
                try
                {
                    var mdn = _dbSet.Where(m => Ids.Contains(m.Id)).ToList();
                    mdn.ForEach(m => { m.IsApproved = true; m.LastModifiedUtc = DateTime.UtcNow; m.LastModifiedAgent = UserAgent; m.LastModifiedBy = _identityInterface.Username; });
                    _inventoryDbContext.SaveChanges();

                    isSuccessful = true;
                    Transaction.Commit();
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    throw ex;
                }
            }

            return isSuccessful;
        }

        public async Task<int> CreateInventoryDocument(MaterialDistributionNote model, string type)
        {
            string storageURI = "master/storages";
            string uomURI = "master/uoms";

            IHttpServiceRepository httpClient = (IHttpServiceRepository)_serviceProvider.GetService(typeof(IHttpServiceRepository));
            /* Get UOM */
            Dictionary<string, object> filterUom = new Dictionary<string, object> { { "Unit", "MTR" } };
            var responseUom = httpClient.GetAsync($@"{APIEndpoint.Core}{uomURI}?filter=" + JsonConvert.SerializeObject(filterUom)).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> resultUom = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseUom.Result);
            var jsonUom = resultUom.Single(p => p.Key.Equals("data")).Value;
            Dictionary<string, object> Uom = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonUom.ToString())[0];

            /* Get Storage */
            var storageName = model.UnitName.Equals("PRINTING") ? "Warehouse Here Printing" : "Warehouse Here Finishing";
            Dictionary<string, object> filterStorage = new Dictionary<string, object> { { "name", storageName } };
            var responseStorage = httpClient.GetAsync($@"{APIEndpoint.Core}{storageURI}?filter=" + JsonConvert.SerializeObject(filterStorage)).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> resultStorage = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseStorage.Result);
            var jsonStorage = resultStorage.Single(p => p.Key.Equals("data")).Value;
            Dictionary<string, object> storage = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonStorage.ToString())[0];

            /* Create Inventory Document */
            List<InventoryDocumentItem> inventoryDocumentItems = new List<InventoryDocumentItem>();
            List<MaterialDistributionNoteDetail> mdnds = new List<MaterialDistributionNoteDetail>();

            foreach (MaterialDistributionNoteItem mdni in model.MaterialDistributionNoteItems)
            {
                mdnds.AddRange(mdni.MaterialDistributionNoteDetails);
            }

            foreach (MaterialDistributionNoteDetail mdnd in mdnds)
            {
                InventoryDocumentItem inventoryDocumentItem = new InventoryDocumentItem
                {
                    ProductId = int.Parse(mdnd.ProductId),
                    ProductCode = mdnd.ProductCode,
                    ProductName = mdnd.ProductName,
                    Quantity = mdnd.ReceivedLength,
                    StockPlanning = model.Type != "RE-GRADING" ? (mdnd.DistributedLength == 0 ? mdnd.MaterialRequestNoteItemLength - mdnd.ReceivedLength : mdnd.ReceivedLength * -1) : mdnd.ReceivedLength * -1,
                    UomId = int.Parse(Uom["Id"].ToString()),
                    UomUnit = Uom["Unit"].ToString()
                };

                inventoryDocumentItems.Add(inventoryDocumentItem);
            }

            List<InventoryDocumentItem> list = inventoryDocumentItems
                    .GroupBy(m => new { m.ProductId, m.ProductCode, m.ProductName })
                    .Select(s => new InventoryDocumentItem
                    {
                        ProductId = s.First().ProductId,
                        ProductCode = s.First().ProductCode,
                        ProductName = s.First().ProductName,
                        Quantity = s.Sum(d => d.Quantity),
                        StockPlanning = s.Sum(d => d.StockPlanning),
                        UomUnit = s.First().UomUnit,
                        UomId = s.First().UomId
                    }).ToList();

            InventoryDocument inventoryDocument = new InventoryDocument
            {
                Date = DateTimeOffset.UtcNow,
                ReferenceNo = model.No,
                ReferenceType = "Bon introduction to Here",
                Type = type,
                StorageId = int.Parse(storage["Id"].ToString()),
                StorageCode = storage["Code"].ToString(),
                StorageName = storage["Name"].ToString(),
                Items = list
            };

            var inventoryDocumentFacade = _serviceProvider.GetService<IInventoryDocumentRepository>();
            return await inventoryDocumentFacade.Create(inventoryDocument);
        }

        public async Task<MaterialDistributionNote> CustomCodeGenerator(MaterialDistributionNote model)
        {
            var type = string.Equals(model.UnitName.ToUpper(), "PRINTING") ? "PR" : "FS";
            var lastData = await this._dbSet.Where(w => w.UnitCode == model.UnitCode).OrderByDescending(o => o.CreatedUtc).FirstOrDefaultAsync();

            DateTime Now = DateTime.Now;
            string Year = Now.ToString("yy");

            if (lastData == null)
            {
                model.AutoIncrementNumber = 1;
                string Number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                model.No = $"P{type}{Year}{Number}";
            }
            else
            {
                if (lastData.CreatedUtc.Year < Now.Year)
                {
                    model.AutoIncrementNumber = 1;
                    string Number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                    model.No = $"P{type}{Year}{Number}";
                }
                else
                {
                    model.AutoIncrementNumber = lastData.AutoIncrementNumber + 1;
                    string Number = model.AutoIncrementNumber.ToString().PadLeft(4, '0');
                    model.No = $"P{type}{Year}{Number}";
                }
            }

            return model;
        }

        public IQueryable<MaterialDistributionNoteReportViewModel> GetReportQuery(string unitId, string type, DateTime date, int offset)
        {
            var query = (from a in _inventoryDbContext.MaterialDistributionNotes
                         join b in _inventoryDbContext.MaterialDistributionNoteItems on a.Id equals b.MaterialDistributionNoteId
                         join c in _inventoryDbContext.MaterialDistributionNoteDetails on b.Id equals c.MaterialDistributionNoteItemId
                         where a.IsDeleted == false
                             && a.UnitId == (string.IsNullOrWhiteSpace(unitId) ? a.UnitId : unitId)
                             && a.Type == (string.IsNullOrWhiteSpace(type) ? a.Type : type)
                             && a.CreatedUtc.AddHours(offset).Date == date.Date
                         select new MaterialDistributionNoteReportViewModel
                         {
                             LastModifiedUtc = a.LastModifiedUtc,
                             No = a.No,
                             Type = a.Type,
                             MaterialRequestNoteNo = b.MaterialRequestNoteCode,
                             ProductionOrderNo = c.ProductionOrderNo,
                             ProductName = c.ProductName,
                             Grade = c.Grade,
                             Quantity = c.Quantity,
                             Length = c.ReceivedLength,
                             SupplierName = c.SupplierName,
                             IsDisposition = a.IsDisposition,
                             IsApproved = a.IsApproved
                         });

            return query;
        }
    }
}
