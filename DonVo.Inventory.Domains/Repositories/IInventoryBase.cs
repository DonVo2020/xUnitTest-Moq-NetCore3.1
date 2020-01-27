using DonVo.Inventory.Infrastructures;
using DonVo.Inventory.Infrastructures.Helpers;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IInventoryBase<InventorySummary, InventorySummaryViewModel> : IMap<InventorySummary, InventorySummaryViewModel>
    {
        Task<int> Create(InventorySummary model);
        ReadResponse<InventorySummary> Read(int page, int size, string order, string keyword, string filter);
        InventorySummary ReadModelById(int id);
    }
}
