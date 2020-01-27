using System;

namespace DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel
{
    public class MaterialsRequestNoteReportViewModel
    {
        public string Code { get; set; }     
        public string OrderNo { get; set; }
        public string ProductName { get; set; }
        public string Grade { get; set; }
        public double OrderQuantity { get; set; }
        public double Length { get; set; }
        public double DistributedLength { get; set; }
        public bool Status { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
