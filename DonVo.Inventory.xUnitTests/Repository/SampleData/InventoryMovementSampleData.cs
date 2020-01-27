using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class InventoryMovementSampleData
    {
        private readonly InventoryMovementRepository _inventoryMovementRepository;

        public InventoryMovementSampleData(InventoryMovementRepository inventoryMovementRepository)
        {
            _inventoryMovementRepository = inventoryMovementRepository;
        }

        public InventoryMovement GetNewData()
        {
            return new InventoryMovement
            {
                No = "No1",
                Date = DateTimeOffset.Now,
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ReferenceNo = "Test001",
                ReferenceType = "TestType",
                ProductCode = "ProductCode1",
                ProductName = "ProductName1",
                UomUnit = "UomUnit1",
                StockPlanning = 1,
                Before = 1,
                Quantity = 1,
                After = 1,
                Type = "IN",
                Remark = "Remark",
            };
        }

        public TViewModel GetNewDataViewModel()
        {
            return new TViewModel
            {
                Date = DateTimeOffset.Now,
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ReferenceNo = "Test001",
                ReferenceType = "TestType",
                ProductCode = "ProductCode1",
                ProductName = "ProductName1",
                UomUnit = "UomUnit1",
                StockPlanning = 1,
                Before = 1,
                Quantity = 1,
                After = 1,
                Type = "IN",
                Remark = "Remark",
            };
        }

        public async Task<InventoryMovement> GetTestData()
        {
            InventoryMovement invMov = GetNewData();

            await _inventoryMovementRepository.Create(invMov);

            return invMov;
        }

        public async Task<InventoryMovement> GetTestDataOUT()
        {
            InventoryMovement invMov = GetNewData();
            invMov.Type = "OUT";

            await _inventoryMovementRepository.Create(invMov);

            return invMov;
        }
    }
}
