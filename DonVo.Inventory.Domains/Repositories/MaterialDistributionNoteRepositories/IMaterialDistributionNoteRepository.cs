using DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel;
using System;
using System.Collections.Generic;

namespace DonVo.Inventory.Domains.Repositories.MaterialDistributionNoteRepositories
{
    public interface IMaterialDistributionNoteRepository
    {
        bool UpdateIsApprove(List<int> Ids);
        Tuple<List<MaterialDistributionNoteReportViewModel>, int> GetReport(string unitId, string type, DateTime date, int page, int size, string Order, int offset);
        List<MaterialDistributionNoteReportViewModel> GetPdfReport(string unitId, string unitName, string type, DateTime date, int offset);
    }
}
