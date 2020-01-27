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
using System.Linq;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public class InventoryDocumentRepository : IInventoryDocumentRepository
    {
        private const string UserAgent = "inventory-service";
        protected DbSet<InventoryDocument> _dbSet;
        public IIdentityInterface _identityInterface;
        public readonly IServiceProvider _serviceProvider;
        public InventoryDbContext _inventoryDbContext;

        public InventoryDocumentRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            _inventoryDbContext = dbContext;
            _serviceProvider = serviceProvider;
            _dbSet = dbContext.Set<InventoryDocument>();
            _identityInterface = serviceProvider.GetService<IIdentityInterface>();
        }

        public async Task<int> Create(InventoryDocument model)
        {
            int created = 0;
            var internalTransaction = _inventoryDbContext.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? _inventoryDbContext.Database.CurrentTransaction : _inventoryDbContext.Database.BeginTransaction();

            try
            {
                model.No = GenerateNo(model);
                model.FlagForCreate(_identityInterface.Username, UserAgent);
                model.FlagForUpdate(_identityInterface.Username, UserAgent);

                foreach (var item in model.Items)
                {
                    item.FlagForCreate(_identityInterface.Username, UserAgent);
                    item.FlagForUpdate(_identityInterface.Username, UserAgent);
                }

                _dbSet.Add(model);
                created = await _inventoryDbContext.SaveChangesAsync();
                foreach (var item in model.Items)
                {
                    var qty = item.Quantity;
                    if (model.Type == "OUT")
                    {
                        qty = item.Quantity * -1;
                    }
                    var sumQty = _inventoryDbContext.InventoryMovements.Where(a => a.IsDeleted == false && a.StorageId == model.StorageId && a.ProductId == item.ProductId && a.UomId == item.UomId).Sum(a => a.Quantity);

                    InventoryMovement movementModel = new InventoryMovement
                    {
                        ProductCode = item.ProductCode,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        StorageCode = model.StorageCode,
                        StorageId = model.StorageId,
                        StorageName = model.StorageName,
                        Before = sumQty,
                        Quantity = qty,
                        After = sumQty + qty,
                        ReferenceNo = model.ReferenceNo,
                        ReferenceType = model.ReferenceType,
                        Type = model.Type,
                        Date = model.Date,
                        UomId = item.UomId,
                        UomUnit = item.UomUnit,
                        Remark = item.ProductRemark
                    };

                    var movement = _serviceProvider.GetService<IInventoryMovementRepository>();
                    await movement.Create(movementModel);
                }
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

        public ReadResponse<InventoryDocument> Read(int page, int size, string order, string keyword, string filter)
        {
            IQueryable<InventoryDocument> query = this._dbSet;

            List<string> searchAttributes = new List<string>()
                {
                    "No", "ReferenceNo", "StorageName","ReferenceType","Type"
                };
            query = QueryHelper<InventoryDocument>.Search(query, searchAttributes, keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "No", "ReferenceNo", "ReferenceType", "Date", "StorageCode", "StorageId", "StorageName", "Type", "LastModifiedUtc", "Items"
            };

            query = query
                .Select(s => new InventoryDocument
                {
                    Id = s.Id,
                    No = s.No,
                    ReferenceNo = s.ReferenceNo,
                    ReferenceType = s.ReferenceType,
                    Date = s.Date,
                    StorageCode = s.StorageCode,
                    StorageId = s.StorageId,
                    StorageName = s.StorageName,
                    Type = s.Type,
                    LastModifiedUtc = s.LastModifiedUtc,
                    Items = s.Items.Select(a => new InventoryDocumentItem
                    {
                        Quantity = a.Quantity,
                        ProductCode = a.ProductCode,
                        ProductId = a.ProductId,
                        ProductName = a.ProductName,
                        UomId = a.UomId,
                        UomUnit = a.UomUnit,
                    }).ToList()
                });


            #region OrderBy

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<InventoryDocument>.Order(query, orderDictionary);
            #endregion OrderBy

            #region Paging

            Pageable<InventoryDocument> pageable = new Pageable<InventoryDocument>(query, page - 1, size);
            List<InventoryDocument> Data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            #endregion Paging

            return new ReadResponse<InventoryDocument>(Data, totalData, orderDictionary, SelectedFields);
        }

        public InventoryDocument ReadModelById(int id)
        {
            var a = this._dbSet.Where(d => d.Id.Equals(id) && d.IsDeleted.Equals(false))
                 .Include(p => p.Items)
                 .FirstOrDefault();
            return a;
        }

        public async Task<int> CreateMulti(List<InventoryDocument> models)
        {
            try
            {
                int created = 0;

                foreach (var item in models)
                {
                    created += await Create(item);
                }
                return created;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public InventoryDocument MapToModel(InventoryDocumentViewModel viewModel)
        {
            InventoryDocument model = new InventoryDocument
            {
                ReferenceNo = viewModel.ReferenceNo,
                ReferenceType = viewModel.ReferenceType,
                Remark = viewModel.Remark,
                StorageCode = viewModel.StorageCode,
                StorageId = viewModel.StorageId,
                StorageName = viewModel.StorageName,
                Date = viewModel.Date,
                Type = viewModel.Type,
                Items = viewModel.Items.Select(item => new InventoryDocumentItem()
                {
                    Id = item.Id,
                    Active = item.Active,
                    CreatedAgent = item.CreatedAgent,
                    CreatedBy = item.CreatedBy,
                    CreatedUtc = item.CreatedUtc,
                    IsDeleted = item.IsDeleted,
                    LastModifiedAgent = item.LastModifiedAgent,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedUtc = item.LastModifiedUtc,
                    ProductCode = item.ProductCode,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductRemark = item.Remark,
                    Quantity = item.Quantity,
                    StockPlanning = item.StockPlanning,
                    UomId = item.UomId,
                    UomUnit = item.Uom,
                }).ToList()
            };
            PropertyCopier<InventoryDocumentViewModel, InventoryDocument>.Copy(viewModel, model);
            return model;
        }

        public InventoryDocumentViewModel MapToViewModel(InventoryDocument model)
        {
            InventoryDocumentViewModel viewModel = new InventoryDocumentViewModel
            {
                No = model.No,
                ReferenceNo = model.ReferenceNo,
                ReferenceType = model.ReferenceType,
                Remark = model.Remark,
                StorageCode = model.StorageCode,
                StorageId = model.StorageId,
                StorageName = model.StorageName,
                Date = model.Date,
                Type = model.Type,
                Items = model.Items.Select(item => new InventoryDocumentItemViewModel()
                {
                    Id = item.Id,
                    Active = item.Active,
                    CreatedAgent = item.CreatedAgent,
                    CreatedBy = item.CreatedBy,
                    CreatedUtc = item.CreatedUtc,
                    IsDeleted = item.IsDeleted,
                    LastModifiedAgent = item.LastModifiedAgent,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedUtc = item.LastModifiedUtc,
                    ProductCode = item.ProductCode,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Remark = item.ProductRemark,
                    Quantity = item.Quantity,
                    StockPlanning = item.StockPlanning,
                    UomId = item.UomId,
                    Uom = item.UomUnit,
                }).ToList()
            };

            PropertyCopier<InventoryDocument, InventoryDocumentViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        private string GenerateNo(InventoryDocument model)
        {
            do
            {
                model.No = CodeGenerator.GenerateCode();
            }
            while (this._dbSet.Any(d => d.No.Equals(model.No)));

            return model.No;
        }   
    }
}
