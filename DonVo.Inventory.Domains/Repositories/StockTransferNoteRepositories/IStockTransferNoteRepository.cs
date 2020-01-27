using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.StockTransferNoteViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories.StockTransferNoteRepositories
{
    public interface IStockTransferNoteRepository : IRepositoryBase<StockTransferNote, StockTransferNoteViewModel>
    {
        Tuple<List<StockTransferNote>, int, Dictionary<string, string>, List<string>> ReadModelByNotUser(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<bool> UpdateIsApprove(int id);
    }
}
