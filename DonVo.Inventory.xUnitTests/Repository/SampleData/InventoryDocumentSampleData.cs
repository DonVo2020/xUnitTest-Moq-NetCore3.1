using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class InventoryDocumentSampleData
    {
        private readonly InventoryDocumentRepository _inventoryDocumentRepository;
        public InventoryDocumentSampleData(InventoryDocumentRepository inventoryDocumentRepository)
        {
            _inventoryDocumentRepository = inventoryDocumentRepository;
        }

        public InventoryDocument GetNewData()
        {
            return new InventoryDocument
            {
                No = "No1",
                Date = DateTimeOffset.Now,
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ReferenceNo = "Test001",
                ReferenceType = "TestType",
                Type = "IN",
                Remark = "Remark",
                Items = new List<InventoryDocumentItem> { new InventoryDocumentItem(){
                    ProductId = 1,
                    ProductCode = "ProductCode",
                    ProductName = "ProductName",
                    Quantity = 10,
                    UomId = 1,
                    UomUnit = "Uom",
                    StockPlanning=0,
                } }
            };
        }

        public InventoryDocumentViewModel GetNewDataViewModel()
        {
            return new InventoryDocumentViewModel
            {
                Date = DateTimeOffset.Now,
                StorageCode = "test01",
                StorageId = 2,
                StorageName = "Test",
                ReferenceNo = "Test001",
                ReferenceType = "TestType",
                Type = "IN",
                Remark = "Remark",
                Items = new List<InventoryDocumentItemViewModel> { new InventoryDocumentItemViewModel(){
                    ProductCode = "ProductCode",
                    ProductName = "ProductName",
                    UomId = 1,
                    Uom = "Uom",
                    Quantity=10,
                    StockPlanning=0,
                } }
            };
        }

        public async Task<InventoryDocument> GetTestData()
        {
            InventoryDocument invDoc = GetNewData();

            await _inventoryDocumentRepository.Create(invDoc);

            return invDoc;
        }

        public async Task<InventoryDocument> GetTestDataOUT()
        {
            InventoryDocument invDoc = GetNewData();
            invDoc.Type = "OUT";

            await _inventoryDocumentRepository.Create(invDoc);

            return invDoc;
        }
    }
}
