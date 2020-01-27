using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class InventorySummarySampleData
    {
        private readonly InventorySummaryRepository _inventorySummaryRepository;

        public InventorySummarySampleData(InventorySummaryRepository inventorySummaryRepository)
        {
            _inventorySummaryRepository = inventorySummaryRepository;
        }

        public InventorySummary GetNewData()
        {
            return new InventorySummary
            {
                No = "No1",
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ProductCode = "ProductCode1",
                ProductName = "ProductName1",
                UomUnit = "UomUnit1",
                StockPlanning = 1,
                Quantity = 1,
            };
        }

        public InventorySummaryViewModel GetNewDataViewModel()
        {
            return new InventorySummaryViewModel
            {
                Code = "No1",
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ProductCode = "ProductCode1",
                ProductName = "ProductName1",
                Quantity = 1,
                StockPlanning = "1",
                Uom = "UomUnit1",
            };
        }

        public async Task<InventorySummary> GetTestData()
        {
            InventorySummary invSum = GetNewData();

            await _inventorySummaryRepository.Create(invSum);

            return invSum;
        }
    }
}
