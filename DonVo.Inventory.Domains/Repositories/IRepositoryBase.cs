using DonVo.Inventory.Infrastructures;
using DonVo.Inventory.Infrastructures.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IRepositoryBase<TModel, TViewModel> : IMap<TModel, TViewModel>
    {
        ReadResponse<TModel> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(TModel model);
        Task<TModel> ReadByIdAsync(int id);
        Task<int> UpdateAsync(int id, TModel model);
        Task<int> DeleteAsync(int id);
    }
}
