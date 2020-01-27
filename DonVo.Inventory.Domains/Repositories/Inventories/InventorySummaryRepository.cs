using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using DonVo.Inventory.Infrastructures.Helpers;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public class InventorySummaryRepository : IInventorySummaryRepository
    {
        private const string UserAgent = "inventory-service";
        protected DbSet<InventorySummary> _dbSet;
        public IIdentityInterface _identityInterface;
        public readonly IServiceProvider _serviceProvider;
        public InventoryDbContext _inventoryDbContext;

        public InventorySummaryRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            _inventoryDbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dbSet = dbContext.Set<InventorySummary>();
            _identityInterface = serviceProvider.GetService<IIdentityInterface>();
        }

        public async Task<int> Create(InventorySummary model)
        {
            int created = 0;
            var internalTransaction = _inventoryDbContext.Database.CurrentTransaction == null;
            var transaction = !internalTransaction 
                                ? _inventoryDbContext.Database.CurrentTransaction 
                                : _inventoryDbContext.Database.BeginTransaction();

            try
            {
                var exist = _dbSet.Where(a => a.IsDeleted == false && a.StorageId == model.StorageId 
                                            && a.ProductId == model.ProductId && a.UomId == model.UomId).FirstOrDefault();

                if (exist == null)
                {
                    model.No = GenerateNo(model);
                    model.FlagForCreate(_identityInterface.Username, UserAgent);
                    model.FlagForUpdate(_identityInterface.Username, UserAgent);

                    _dbSet.Add(model);
                }
                else
                {
                    model.FlagForUpdate(_identityInterface.Username, UserAgent);

                    exist.Quantity = model.Quantity;
                    exist.StockPlanning = model.StockPlanning;
                    this._dbSet.Update(exist);
                }
                created = await _inventoryDbContext.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return created;
            }
            catch (Exception e)
            {
                if (internalTransaction)
                    transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        public InventorySummary ReadModelById(int id)
        {
            var a = _dbSet.Where(d => d.Id.Equals(id) && d.IsDeleted.Equals(false))
                .FirstOrDefault();
            return a;
        }

        public ReadResponse<InventorySummary> Read(int page, int size, string order, string keyword, string filter)
        {
            IQueryable<InventorySummary> query = _dbSet;

            query = query
                .Select(s => new InventorySummary
                {
                    Id = s.Id,
                    No = s.No,
                    StorageCode = s.StorageCode,
                    StorageId = s.StorageId,
                    StorageName = s.StorageName,
                    ProductCode = s.ProductCode,
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    Quantity = s.Quantity,
                    StockPlanning = s.StockPlanning
                });

            List<string> searchAttributes = new List<string>()
            {
                "No", "ReferenceNo", "StorageName","ReferenceType"
            };

            List<string> selectedFields = new List<string>()
            {
                "Id", "No", "StorageCode", "StorageId", "StorageName", "ProductCode", "ProductId", "ProductName",
                "Quantity", "StockPlanning"
            };

            query = QueryHelper<InventorySummary>.Search(query, searchAttributes, keyword);
            #region OrderBy

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<InventorySummary>.Order(query, orderDictionary);


            #endregion OrderBy

            #region Paging

            Pageable<InventorySummary> pageable = new Pageable<InventorySummary>(query, page - 1, size);
            List<InventorySummary> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            #endregion Paging

            return new ReadResponse<InventorySummary>(data, totalData, orderDictionary, selectedFields);
        }

        public List<InventorySummary> GetByStorageAndMTR(string storageName)
        {
            IQueryable<InventorySummary> query = _dbSet;

            query = query.Where(s => s.StorageName.Equals(storageName) && s.UomUnit.Equals("MTR"));

            return query.ToList();
        }
        
        public Tuple<List<InventorySummaryViewModel>, int> GetReport(string storageCode, string productCode, int page, int size, string order, int offset)
        {
            var query = GetReportQuery(storageCode, productCode, offset);

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            if (orderDictionary.Count.Equals(0))
            {
                query = query.OrderByDescending(b => b.LastModifiedUtc);
            }
            else
            {
                string key = orderDictionary.Keys.First();
                string OrderType = orderDictionary[key];
                query = query.OrderBy(string.Concat(key, " ", OrderType));
            }

            Pageable<InventorySummaryViewModel> pageable = new Pageable<InventorySummaryViewModel>(query, page - 1, size);
            List<InventorySummaryViewModel> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return Tuple.Create(data, totalData);
        }

        public List<InventorySummaryViewModel> GetInventorySummaries(string productIds = "{}")
        {
            Dictionary<string, object> productIdDictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(productIds);
            if (productIdDictionaries.Count == 0)
                return new List<InventorySummaryViewModel>();
            var productIdString = productIdDictionaries.Values.FirstOrDefault();
            var productIdList = JsonConvert.DeserializeObject<List<int>>(productIdString.ToString());

            IQueryable<InventorySummary> rootQuery = _inventoryDbContext.InventorySummaries.Where(x => !x.IsDeleted);
            List<InventorySummary> documentItems = new List<InventorySummary>();
            foreach (var id in productIdList)
            {
                var result = rootQuery.Where(x => x.ProductId == id);
                documentItems.AddRange(result.ToList());
            }
            return documentItems.Select(x => new InventorySummaryViewModel()
            {
                ProductCode = x.ProductCode,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                StorageCode = x.StorageCode,
                Active = x.Active,
                Id = x.Id,
                StockPlanning = x.StockPlanning.ToString(),
                StorageId = x.StorageId,
                StorageName = x.StorageName,
                Code = x.No,
                UomId = x.UomId,
                Uom = x.UomUnit,
            }).ToList();
        }

        public InventorySummary GetSummaryByParams(int storageId, int productId, int uomId)
        {
            return _dbSet.FirstOrDefault(f => f.StorageId.Equals(storageId) && f.ProductId.Equals(productId) && f.UomId.Equals(uomId));
        }

        public MemoryStream GenerateExcel(string storageCode, string productCode, int offset)
        {
            var query = GetReportQuery(storageCode, productCode, offset);
            query = query.OrderByDescending(b => b.LastModifiedUtc);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Storage", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Item name", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Uom", DataType = typeof(string) });
            if (query.ToArray().Count() == 0)
                result.Rows.Add(string.Empty, string.Empty, string.Empty, 0, string.Empty); // to allow column name to be generated properly for empty data as template
            else
            {
                int index = 0;
                foreach (var item in query)
                {
                    index++;
                    result.Rows.Add(index, item.StorageName, item.ProductName, item.Quantity, item.Uom);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") }, true);
        }

        private IQueryable<InventorySummaryViewModel> GetReportQuery(string storageCode, string productCode, int offset)
        {
            var query = (from a in _inventoryDbContext.InventorySummaries
                             //Conditions
                         where a.IsDeleted == false
                             && a.StorageCode == (string.IsNullOrWhiteSpace(storageCode) ? a.StorageCode : storageCode)
                             && a.ProductCode == (string.IsNullOrWhiteSpace(productCode) ? a.ProductCode : productCode)
                         select new InventorySummaryViewModel
                         {
                             Code = a.No,
                             ProductId = a.ProductId,
                             ProductCode = a.ProductCode,
                             ProductName = a.ProductName,
                             UomId = a.UomId,
                             Uom = a.UomUnit,
                             StorageId = a.StorageId,
                             StorageCode = a.StorageCode,
                             StorageName = a.StorageName,
                             Quantity = a.Quantity,

                             LastModifiedUtc = a.LastModifiedUtc
                         });
            return query;
        }

        private string GenerateNo(InventorySummary model)
        {
            do
            {
                model.No = CodeGenerator.GenerateCode();
            }
            while (this._dbSet.Any(d => d.No.Equals(model.No)));

            return model.No;
        }

        public InventorySummaryViewModel MapToViewModel(InventorySummary model)
        {
            var viewModel = new InventorySummaryViewModel()
            {
                Code = model.No,
                ProductId = model.ProductId,
                ProductCode = model.ProductCode,
                ProductName = model.ProductName,
                UomId = model.UomId,
                Uom = model.UomUnit,
                StorageId = model.StorageId,
                StorageCode = model.StorageCode,
                StorageName = model.StorageName,
                StockPlanning = model.StockPlanning.ToString(),
                Quantity = model.Quantity
            };

            PropertyCopier<InventorySummary, InventorySummaryViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public InventorySummary MapToModel(InventorySummaryViewModel viewModel)
        {
            var model = new InventorySummary()
            {
                No = viewModel.Code,
                ProductId = viewModel.ProductId,
                ProductCode = viewModel.ProductCode,
                ProductName = viewModel.ProductName,
                UomId = viewModel.UomId,
                UomUnit = viewModel.Uom,
                StorageId = viewModel.StorageId,
                StorageCode = viewModel.StorageCode,
                StorageName = viewModel.StorageName,
                StockPlanning = double.Parse(viewModel.StockPlanning),
                Quantity = viewModel.Quantity
            };

            PropertyCopier<InventorySummaryViewModel, InventorySummary>.Copy(viewModel, model);
            return model;
        }
    }
}
