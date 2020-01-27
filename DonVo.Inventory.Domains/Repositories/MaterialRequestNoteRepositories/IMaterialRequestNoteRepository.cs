using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories
{
    public interface IMaterialRequestNoteRepository : IRepositoryBase<MaterialsRequestNote, MaterialsRequestNoteViewModel>
    {
        Task UpdateIsCompleted(int Id, MaterialsRequestNote Model);
        Tuple<List<MaterialsRequestNoteReportViewModel>, int> GetReport(string materialsRequestNoteCode, string productionOrderId, string unitId, string productId, string status, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset);
        void UpdateDistributedQuantity(int Id, MaterialsRequestNote Model);
    }
}
