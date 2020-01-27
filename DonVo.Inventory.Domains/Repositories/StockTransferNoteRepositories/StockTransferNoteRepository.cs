using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using DonVo.Inventory.Infrastructures.Helpers;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using DonVo.Inventory.ViewModels.StockTransferNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories.StockTransferNoteRepositories
{
    public class StockTransferNoteRepository : IStockTransferNoteRepository
    {
        private const string UserAgent = "inventory-repository";
        protected DbSet<StockTransferNote> DbSet;
        public IIdentityInterface IdentityInterface;
        public readonly IServiceProvider ServiceProvider;
        public InventoryDbContext DbContext;

        public StockTransferNoteRepository(IServiceProvider serviceProvider, InventoryDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<StockTransferNote>();
            IdentityInterface = serviceProvider.GetService<IIdentityInterface>();
        }

        public async Task<int> CreateAsync(StockTransferNote model)
        {
            int Created = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    model.Code = CodeGenerator.GenerateCode();
                    model.FlagForCreate(IdentityInterface.Username, UserAgent);
                    model.FlagForUpdate(IdentityInterface.Username, UserAgent);
                    foreach (var item in model.StockTransferNoteItems)
                    {
                        item.FlagForCreate(IdentityInterface.Username, UserAgent);
                        item.FlagForUpdate(IdentityInterface.Username, UserAgent);
                    }

                    DbSet.Add(model);

                    Created = await DbContext.SaveChangesAsync();
                    await CreateInventoryDocument(model, "OUT", "CREATE");

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return Created;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int Count = 0;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    StockTransferNote stockTransferNote = await ReadByIdAsync(id);
                    stockTransferNote.FlagForDelete(IdentityInterface.Username, UserAgent);
                    foreach (var item in stockTransferNote.StockTransferNoteItems)
                    {
                        item.FlagForDelete(IdentityInterface.Username, UserAgent);
                    }
                    Count = await DbContext.SaveChangesAsync();


                    //StockTransferNoteService stockTransferNoteService = ServiceProvider.GetService<StockTransferNoteService>();
                    //StockTransferNoteItemService stockTransferNoteItemService = ServiceProvider.GetService<StockTransferNoteItemService>();
                    //stockTransferNoteItemService.Username = this.Username;

                    //HashSet<int> StockTransferNoteItems = new HashSet<int>(this.DbContext.StockTransferNoteItems.Where(p => p.StockTransferNote.Equals(Id)).Select(p => p.Id));

                    //foreach (int item in StockTransferNoteItems)
                    //{
                    //    await stockTransferNoteItemService.DeleteAsync(item);
                    //}

                    await CreateInventoryDocument(stockTransferNote, "IN", "DELETE-SOURCE");

                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    throw e;
                }
            }

            return Count;
        }

        public StockTransferNote MapToModel(StockTransferNoteViewModel viewModel)
        {
            StockTransferNote model = new StockTransferNote();
            PropertyCopier<StockTransferNoteViewModel, StockTransferNote>.Copy(viewModel, model);

            model.SourceStorageId = viewModel.SourceStorage.Id;
            model.SourceStorageCode = viewModel.SourceStorage.Code;
            model.SourceStorageName = viewModel.SourceStorage.Name;
            model.TargetStorageId = viewModel.TargetStorage.Id;
            model.TargetStorageCode = viewModel.TargetStorage.Code;
            model.TargetStorageName = viewModel.TargetStorage.Name;

            model.StockTransferNoteItems = new List<StockTransferNoteItem>();
            foreach (StockTransferNoteItemViewModel stn in viewModel.StockTransferNoteItems)
            {
                StockTransferNoteItem stockTransferNoteItem = new StockTransferNoteItem();
                PropertyCopier<StockTransferNoteItemViewModel, StockTransferNoteItem>.Copy(stn, stockTransferNoteItem);

                stockTransferNoteItem.ProductId = stn.Summary.ProductId.ToString();
                stockTransferNoteItem.ProductCode = stn.Summary.ProductCode;
                stockTransferNoteItem.ProductName = stn.Summary.ProductName;
                stockTransferNoteItem.UomId = stn.Summary.UomId.ToString();
                stockTransferNoteItem.UomUnit = stn.Summary.Uom;
                stockTransferNoteItem.StockQuantity = stn.Summary.Quantity;
                stockTransferNoteItem.TransferedQuantity = stn.TransferedQuantity != null ? (double)stn.TransferedQuantity : 0;

                model.StockTransferNoteItems.Add(stockTransferNoteItem);
            }

            return model;
        }

        public StockTransferNoteViewModel MapToViewModel(StockTransferNote model)
        {
            StockTransferNoteViewModel viewModel = new StockTransferNoteViewModel();
            PropertyCopier<StockTransferNote, StockTransferNoteViewModel>.Copy(model, viewModel);

            CodeNameViewModel SourceStorage = new CodeNameViewModel()
            {
                Id = model.SourceStorageId,
                Code = model.SourceStorageCode,
                Name = model.SourceStorageName
            };

            CodeNameViewModel TargetStorage = new CodeNameViewModel()
            {
                Id = model.TargetStorageId,
                Code = model.TargetStorageCode,
                Name = model.TargetStorageName
            };

            viewModel.SourceStorage = SourceStorage;
            viewModel.TargetStorage = TargetStorage;

            viewModel.StockTransferNoteItems = new List<StockTransferNoteItemViewModel>();
            if (model.StockTransferNoteItems != null)
            {
                foreach (StockTransferNoteItem stn in model.StockTransferNoteItems)
                {
                    StockTransferNoteItemViewModel stockTransferNoteItemViewModel = new StockTransferNoteItemViewModel();
                    PropertyCopier<StockTransferNoteItem, StockTransferNoteItemViewModel>.Copy(stn, stockTransferNoteItemViewModel);
                    InventorySummaryViewModel Summary = new InventorySummaryViewModel()
                    {
                        ProductId = int.TryParse(stn.ProductId, out int prdId) ? prdId : 0,
                        ProductCode = stn.ProductCode,
                        ProductName = stn.ProductName,
                        Quantity = stn.StockQuantity,
                        UomId = int.TryParse(stn.UomId, out int unitId) ? unitId : 0,
                        Uom = stn.UomUnit
                    };

                    stockTransferNoteItemViewModel.Summary = Summary;

                    viewModel.StockTransferNoteItems.Add(stockTransferNoteItemViewModel);
                }
            }

            return viewModel;
        }

        public ReadResponse<StockTransferNote> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<StockTransferNote> Query = this.DbContext.StockTransferNotes;

            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<StockTransferNote>.Search(Query, SearchAttributes, keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Code", "_CreatedUtc", "ReferenceNo", "ReferenceType", "SourceStorage", "TargetStorage", "IsApproved", "_LastModifiedUtc"
            };

            Query = Query
                .Select(stn => new StockTransferNote
                {
                    Id = stn.Id,
                    Code = stn.Code,
                    CreatedUtc = stn.CreatedUtc,
                    ReferenceNo = stn.ReferenceNo,
                    ReferenceType = stn.ReferenceType,
                    SourceStorageName = stn.SourceStorageName,
                    TargetStorageName = stn.TargetStorageName,
                    IsApproved = stn.IsApproved,
                    LastModifiedUtc = stn.LastModifiedUtc
                });

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<StockTransferNote>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<StockTransferNote>.Order(Query, OrderDictionary);

            Pageable<StockTransferNote> pageable = new Pageable<StockTransferNote>(Query, page - 1, size);
            List<StockTransferNote> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<StockTransferNote>(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<StockTransferNote> ReadByIdAsync(int id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(id) && d.IsDeleted.Equals(false))
                .Include(d => d.StockTransferNoteItems)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(int id, StockTransferNote model)
        {
            int updated = 0;
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (id != model.Id)
                        throw new Exception("data not found");

                    var dbModel = await ReadByIdAsync(id);

                    dbModel.IsApproved = model.IsApproved;
                    dbModel.ReferenceNo = model.ReferenceNo;
                    dbModel.ReferenceType = model.ReferenceType;
                    dbModel.SourceStorageCode = model.SourceStorageCode;
                    dbModel.SourceStorageId = model.SourceStorageId;
                    dbModel.SourceStorageName = model.SourceStorageName;
                    dbModel.TargetStorageCode = model.TargetStorageCode;
                    dbModel.TargetStorageId = model.TargetStorageId;
                    dbModel.TargetStorageName = model.TargetStorageName;

                    dbModel.FlagForUpdate(IdentityInterface.Username, UserAgent);
                    //DbSet.Update(dbModel);

                    var deletedItems = dbModel.StockTransferNoteItems.Where(x => !model.StockTransferNoteItems.Any(y => x.Id == y.Id));
                    var updatedItems = dbModel.StockTransferNoteItems.Where(x => model.StockTransferNoteItems.Any(y => x.Id == y.Id));
                    var addedItems = model.StockTransferNoteItems.Where(x => !dbModel.StockTransferNoteItems.Any(y => y.Id == x.Id));

                    foreach (var item in deletedItems)
                    {
                        item.FlagForDelete(IdentityInterface.Username, UserAgent);

                    }

                    foreach (var item in updatedItems)
                    {
                        var selectedItem = model.StockTransferNoteItems.FirstOrDefault(x => x.Id == item.Id);

                        item.ProductCode = selectedItem.ProductCode;
                        item.ProductId = selectedItem.ProductId;
                        item.ProductName = selectedItem.ProductName;
                        item.StockQuantity = selectedItem.StockQuantity;
                        item.TransferedQuantity = selectedItem.TransferedQuantity;
                        item.UomId = selectedItem.UomId;
                        item.UomUnit = selectedItem.UomUnit;
                        item.FlagForUpdate(IdentityInterface.Username, UserAgent);

                    }

                    foreach (var item in addedItems)
                    {
                        item.StockTransferNoteId = id;
                        item.FlagForCreate(IdentityInterface.Username, UserAgent);
                        item.FlagForUpdate(IdentityInterface.Username, UserAgent);

                    }
                    updated = await DbContext.SaveChangesAsync();
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

        public Tuple<List<StockTransferNote>, int, Dictionary<string, string>, List<string>> ReadModelByNotUser(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            IQueryable<StockTransferNote> Query = this.DbContext.StockTransferNotes;

            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<StockTransferNote>.Search(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Code", "_CreatedUtc", "ReferenceNo", "ReferenceType", "SourceStorage", "TargetStorage", "IsApproved", "_LastModifiedUtc", "_CreatedBy"
            };

            Query = Query
                .Select(stn => new StockTransferNote
                {
                    Id = stn.Id,
                    Code = stn.Code,
                    CreatedUtc = stn.CreatedUtc,
                    ReferenceNo = stn.ReferenceNo,
                    ReferenceType = stn.ReferenceType,
                    SourceStorageName = stn.SourceStorageName,
                    TargetStorageName = stn.TargetStorageName,
                    IsApproved = stn.IsApproved,
                    LastModifiedUtc = stn.LastModifiedUtc,
                    CreatedBy = stn.CreatedBy
                }).Where(w => !string.Equals(w.CreatedBy, IdentityInterface.Username));

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = QueryHelper<StockTransferNote>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<StockTransferNote>.Order(Query, OrderDictionary);

            Pageable<StockTransferNote> pageable = new Pageable<StockTransferNote>(Query, Page - 1, Size);
            List<StockTransferNote> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<int> CreateInventoryDocument(StockTransferNote Model, string Type, string Context)
        {
            StockTransferNoteViewModel ViewModel = MapToViewModel(Model);

            IHttpServiceRepository httpClient = (IHttpServiceRepository)this.ServiceProvider.GetService(typeof(IHttpServiceRepository));

            /* Create Inventory Document */
            List<InventoryDocumentItem> inventoryDocumentItems = new List<InventoryDocumentItem>();

            foreach (StockTransferNoteItemViewModel stni in ViewModel.StockTransferNoteItems)
            {
                InventoryDocumentItem inventoryDocumentItem = new InventoryDocumentItem
                {
                    ProductId = stni.Summary.ProductId,
                    ProductCode = stni.Summary.ProductCode,
                    ProductName = stni.Summary.ProductName,
                    Quantity = stni.TransferedQuantity != null ? (double)stni.TransferedQuantity : 0,
                    UomId = stni.Summary.UomId,
                    UomUnit = stni.Summary.Uom
                };

                inventoryDocumentItems.Add(inventoryDocumentItem);
            }

            InventoryDocument inventoryDocument = new InventoryDocument
            {
                Date = DateTimeOffset.UtcNow,
                ReferenceNo = Model.ReferenceNo,
                ReferenceType = Model.ReferenceType,
                Type = Type,
                StorageId = string.Equals(Context.ToUpper(), "CREATE") || string.Equals(Context.ToUpper(), "DELETE-SOURCE") ? int.Parse(Model.SourceStorageId) : int.Parse(Model.TargetStorageId),
                StorageCode = string.Equals(Context.ToUpper(), "CREATE") || string.Equals(Context.ToUpper(), "DELETE-SOURCE") ? Model.SourceStorageCode : Model.TargetStorageCode,
                StorageName = string.Equals(Context.ToUpper(), "CREATE") || string.Equals(Context.ToUpper(), "DELETE-SOURCE") ? Model.SourceStorageName : Model.TargetStorageName,
                Items = inventoryDocumentItems
            };

            var inventoryDocumentFacade = ServiceProvider.GetService<IInventoryDocumentRepository>();
            return await inventoryDocumentFacade.Create(inventoryDocument);
        }

        public async Task<bool> UpdateIsApprove(int Id)
        {
            bool IsSuccessful = false;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var stockTransferNote = await this.ReadByIdAsync(Id);
                    stockTransferNote.IsApproved = true;
                    stockTransferNote.FlagForUpdate(IdentityInterface.Username, UserAgent);
                    this.DbContext.SaveChanges();
                    await CreateInventoryDocument(stockTransferNote, "IN", "APPROVE");

                    IsSuccessful = true;
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    throw e;
                }
            }

            return IsSuccessful;
        }
    }
}
