using System;

namespace DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel
{
    public class MaterialDistributionNoteReportViewModel
    {
        public string No { get; set; }
        public string Type { get; set; }
        public string MaterialRequestNoteNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string ProductName { get; set; }
        public string Grade { get; set; }
        public double Quantity { get; set; }
        public double Length { get; set; }
        public string SupplierName { get; set; }
        public bool IsDisposition { get; set; }
        public bool IsApproved { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}
