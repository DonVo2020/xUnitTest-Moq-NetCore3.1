using DonVo.Inventory.Domains.Repositories.StockTransferNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.StockTransferNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class StockTransferNoteSampleData
    {
        private readonly StockTransferNoteRepository _repository;

        public StockTransferNoteSampleData(StockTransferNoteRepository repository)
        {
            _repository = repository;
        }

        public StockTransferNoteViewModel GetEmptyData()
        {
            StockTransferNoteViewModel data = new StockTransferNoteViewModel
            {
                ReferenceNo = string.Empty,
                ReferenceType = string.Empty,
                SourceStorage = new CodeNameViewModel(),
                TargetStorage = new CodeNameViewModel(),
                StockTransferNoteItems = new List<StockTransferNoteItemViewModel> { new StockTransferNoteItemViewModel() }
            };

            return data;
        }

        public StockTransferNote GetNewData()
        {
            StockTransferNote testData = new StockTransferNote
            {
                ReferenceNo = "Reference No Test",
                ReferenceType = "Reference Type Test",
                SourceStorageId = "1",
                SourceStorageCode = "code",
                SourceStorageName = "name",
                TargetStorageId = "1",
                TargetStorageCode = "code",
                TargetStorageName = "name",
                IsApproved = false,
                StockTransferNoteItems = new List<StockTransferNoteItem> { new StockTransferNoteItem(){
                    ProductCode = "code",
                    ProductId = "1",
                    ProductName = "name",
                    StockQuantity = 1,
                    TransferedQuantity = 1,
                    UomId = "1",
                    UomUnit = "unit"
                } }
            };

            return testData;
        }

        public async Task<StockTransferNote> GetTestData()
        {
            StockTransferNote data = GetNewData();
            await _repository.CreateAsync(data);
            return data;
        }
    }
}
