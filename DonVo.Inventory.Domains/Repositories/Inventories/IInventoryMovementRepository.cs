using DonVo.Inventory.Domains.Mapping;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IInventoryMovementRepository : IInventoryBase<InventoryMovement, TViewModel>, IMappingModelViewModel<InventoryMovement, TViewModel>
    {
        Tuple<List<TViewModel>, int> GetReport(string storageCode, string productCode, string type, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset);
        MemoryStream GenerateExcel(string storageCode, string productCode, string type, DateTime? dateFrom, DateTime? dateTo, int offset);
        Task<int> RefreshInventoryMovement();
    }
}
