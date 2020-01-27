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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private const string UserAgent = "inventory-service";
        protected DbSet<InventoryMovement> _dbSet;
        public IIdentityInterface _identityInterface;
        public readonly IServiceProvider _serviceProvider;
        public InventoryDbContext _inventoryDbContext;

        public InventoryMovementRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            _inventoryDbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dbSet = dbContext.Set<InventoryMovement>();
            _identityInterface = serviceProvider.GetService<IIdentityInterface>();
        }
        public async Task<int> Create(InventoryMovement model)
        {
            int created = 0;

            var internalTransaction = _inventoryDbContext.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? _inventoryDbContext.Database.CurrentTransaction : _inventoryDbContext.Database.BeginTransaction();

            try
            {
                model.No = GenerateNo(model);
                model.FlagForCreate(_identityInterface.Username, UserAgent);
                model.FlagForUpdate(_identityInterface.Username, UserAgent);

                _dbSet.Add(model);
                created = await _inventoryDbContext.SaveChangesAsync();

                var sumQty = _dbSet.OrderByDescending(a => a.CreatedUtc).FirstOrDefault(a => a.IsDeleted == false && a.StorageId == model.StorageId && a.ProductId == model.ProductId && a.UomId == model.UomId);

                var sumStock = this._dbSet.Where(a => a.IsDeleted == false && a.StorageId == model.StorageId && a.ProductId == model.ProductId && a.UomId == model.UomId).Sum(a => a.StockPlanning);
                InventorySummary summaryModel = new InventorySummary
                {
                    ProductId = model.ProductId,
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    UomId = model.UomId,
                    UomUnit = model.UomUnit,
                    StockPlanning = sumStock,
                    Quantity = sumQty.After,
                    StorageId = model.StorageId,
                    StorageCode = model.StorageCode,
                    StorageName = model.StorageName
                };

                var summary = _serviceProvider.GetService<IInventorySummaryRepository>();
                await summary.Create(summaryModel);
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

        public ReadResponse<InventoryMovement> Read(int page, int size, string order, string keyword, string filter)
        {
            IQueryable<InventoryMovement> query = this._dbSet;
            List<string> selectedFields = new List<string>()
            {
                "Id", "No", "ReferenceNo", "ReferenceType", "Date", "StorageCode", "StorageId", "StorageName",
                "ProductCode", "ProductId", "ProductName", "Quantity", "StockPlanning", "Before", "After"
            };
            query = query
                .Select(s => new InventoryMovement
                {
                    Id = s.Id,
                    No = s.No,
                    ReferenceNo = s.ReferenceNo,
                    ReferenceType = s.ReferenceType,
                    Date = s.Date,
                    StorageCode = s.StorageCode,
                    StorageId = s.StorageId,
                    StorageName = s.StorageName,
                    ProductCode = s.ProductCode,
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    Quantity = s.Quantity,
                    StockPlanning = s.StockPlanning,
                    Before = s.Before,
                    After = s.After
                });

            List<string> searchAttributes = new List<string>()
            {
                "No", "ReferenceNo", "StorageName","ReferenceType"
            };

            query = QueryHelper<InventoryMovement>.Search(query, searchAttributes, keyword);
            #region OrderBy

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<InventoryMovement>.Order(query, orderDictionary);

            #endregion OrderBy

            #region Paging

            Pageable<InventoryMovement> pageable = new Pageable<InventoryMovement>(query, page - 1, size);
            List<InventoryMovement> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            #endregion Paging

            return new ReadResponse<InventoryMovement>(data, totalData, orderDictionary, selectedFields);
        }

        public InventoryMovement ReadModelById(int id)
        {
            var a = this._dbSet.Where(d => d.Id.Equals(id) && d.IsDeleted.Equals(false))
                .FirstOrDefault();
            return a;
        }

        public Tuple<List<TViewModel>, int> GetReport(string storageCode, string productCode, string type, DateTime? dateFrom, DateTime? dateTo, int page, int size, string order, int offset)
        {
            var query = GetReportQuery(storageCode, productCode, type, dateFrom, dateTo, offset);

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

            Pageable<TViewModel> pageable = new Pageable<TViewModel>(query, page - 1, size);
            List<TViewModel> data = pageable.Data.ToList<TViewModel>();
            int totalData = pageable.TotalCount;

            return Tuple.Create(data, totalData);
        }

        public MemoryStream GenerateExcel(string storageCode, string productCode, string type, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            var query = GetReportQuery(storageCode, productCode, type, dateFrom, dateTo, offset);
            query = query.OrderByDescending(b => b.LastModifiedUtc);
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Storage", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Reference number", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "RequestType Reference", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Date", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Item name", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Uom", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Before", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "After", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Type", DataType = typeof(string) });
            if (query.ToArray().Count() == 0)
                result.Rows.Add(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, string.Empty); // to allow column name to be generated properly for empty data as template
            else
            {
                int index = 0;
                foreach (var item in query)
                {
                    index++;
                    //DateTimeOffset date = item.date ?? new DateTime(1970, 1, 1);
                    //string dateString = date == new DateTime(1970, 1, 1) ? "-" : date.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    result.Rows.Add(index, item.StorageName, item.ReferenceNo, item.ReferenceType, item.Date.ToString("dd MMM yyyy", new CultureInfo("id-ID")), item.ProductName, item.UomUnit, item.Before,
                        item.Quantity, item.After, item.Type);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") }, true);
        }

        public async Task<int> RefreshInventoryMovement()
        {
            using var transaction = _inventoryDbContext.Database.BeginTransaction();
            try
            {
                //int result = 0;
                List<InventoryMovement> dbMovement = await _inventoryDbContext.InventoryMovements.ToListAsync();

                foreach (var groupedItem in dbMovement.GroupBy(x => new { x.StorageId, x.ProductId, x.UomId }).ToList())
                {
                    var orderedItem = groupedItem.OrderBy(x => x.CreatedUtc).ThenBy(x => x.Id).ToList();
                    //result += orderedItem.Count;
                    for (int i = 1; i < orderedItem.Count; i++)
                    {
                        var item = orderedItem[i];
                        var previousItem = orderedItem[i - 1];
                        item.Before = previousItem.After;
                        item.After = item.Before + item.Quantity;
                    }
                }
                _inventoryDbContext.UpdateRange(dbMovement);
                var result = await _inventoryDbContext.SaveChangesAsync();
                transaction.Commit();

                return result;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
            finally
            {
                _inventoryDbContext.Dispose();
            }
        }

        public InventoryMovement MapToModel(TViewModel viewModel)
        {
            var model = new InventoryMovement()
            {
                No = viewModel.No,
                Date = viewModel.Date,
                ReferenceNo = viewModel.ReferenceNo,
                ReferenceType = viewModel.ReferenceType,
                ProductId = viewModel.ProductId,
                ProductCode = viewModel.ProductCode,
                ProductName = viewModel.ProductName,
                UomId = viewModel.UomId,
                UomUnit = viewModel.UomUnit,
                StorageId = viewModel.StorageId,
                StorageCode = viewModel.StorageCode,
                StorageName = viewModel.StorageName,
                StockPlanning = viewModel.StockPlanning,
                Before = viewModel.Before,
                Quantity = viewModel.Quantity,
                After = viewModel.After,
                Remark = viewModel.Remark,
                Type = viewModel.Type
            };

            PropertyCopier<TViewModel, InventoryMovement>.Copy(viewModel, model);
            return model;
        }

        public TViewModel MapToViewModel(InventoryMovement model)
        {
            var viewModel = new TViewModel()
            {
                No = model.No,
                Date = model.Date,
                ReferenceNo = model.ReferenceNo,
                ReferenceType = model.ReferenceType,
                ProductId = model.ProductId,
                ProductCode = model.ProductCode,
                ProductName = model.ProductName,
                UomId = model.UomId,
                UomUnit = model.UomUnit,
                StorageId = model.StorageId,
                StorageCode = model.StorageCode,
                StorageName = model.StorageName,
                StockPlanning = model.StockPlanning,
                Before = model.Before,
                Quantity = model.Quantity,
                After = model.After,
                Remark = model.Remark,
                Type = model.Type,
                LastModifiedUtc = model.LastModifiedUtc
            };
            PropertyCopier<InventoryMovement, TViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        private string GenerateNo(InventoryMovement model)
        {
            do
            {
                model.No = CodeGenerator.GenerateCode();
            }
            while (this._dbSet.Any(d => d.No.Equals(model.No)));

            return model.No;
        }

        private IQueryable<TViewModel> GetReportQuery(string storageCode, string productCode, string type, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var query = (from a in _inventoryDbContext.InventoryMovements
                             //Conditions
                         where a.IsDeleted == false
                             && a.StorageCode == (string.IsNullOrWhiteSpace(storageCode) ? a.StorageCode : storageCode)
                             && a.ProductCode == (string.IsNullOrWhiteSpace(productCode) ? a.ProductCode : productCode)
                             && a.Type == (string.IsNullOrWhiteSpace(type) ? a.Type : type)
                             && a.Date.AddHours(offset).Date >= DateFrom.Date
                             && a.Date.AddHours(offset).Date <= DateTo.Date
                         select new TViewModel
                         {
                             No = a.No,
                             Date = a.Date,
                             ReferenceNo = a.ReferenceNo,
                             ReferenceType = a.ReferenceType,
                             ProductId = a.ProductId,
                             ProductCode = a.ProductCode,
                             ProductName = a.ProductName,
                             UomId = a.UomId,
                             UomUnit = a.UomUnit,
                             StorageId = a.StorageId,
                             StorageCode = a.StorageCode,
                             StorageName = a.StorageName,
                             StockPlanning = a.StockPlanning,
                             Before = a.Before,
                             Quantity = a.Quantity,
                             After = a.After,
                             Remark = a.Remark,
                             Type = a.Type,
                             LastModifiedUtc = a.LastModifiedUtc
                         });
            return query;
        }
    }
}
