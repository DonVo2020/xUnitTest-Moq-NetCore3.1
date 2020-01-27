using DonVo.Inventory.Domains.Mapping;
using DonVo.Inventory.Infrastructures.Helpers;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IInventoryDocumentRepository : IMappingModelViewModel<InventoryDocument, InventoryDocumentViewModel>
    {
        Task<int> CreateMulti(List<InventoryDocument> models);
        Task<int> Create(InventoryDocument model);
        ReadResponse<InventoryDocument> Read(int page, int size, string order, string keyword, string filter);
        InventoryDocument ReadModelById(int id);
    }
}
