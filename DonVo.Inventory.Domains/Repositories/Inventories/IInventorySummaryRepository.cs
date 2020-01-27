using DonVo.Inventory.Domains.Mapping;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IInventorySummaryRepository : IInventoryBase<InventorySummary, InventorySummaryViewModel>, IMappingModelViewModel<InventorySummary, InventorySummaryViewModel>
    {
        List<InventorySummary> GetByStorageAndMTR(string storageName);
        Tuple<List<InventorySummaryViewModel>, int> GetReport(string storageCode, string productCode, int page, int size, string Order, int offset);
        List<InventorySummaryViewModel> GetInventorySummaries(string productIds = "{}");
        InventorySummary GetSummaryByParams(int storageId, int productId, int uomId);
        MemoryStream GenerateExcel(string storageCode, string productCode, int offset);
    }
}
