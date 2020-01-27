using System;

namespace DonVo.Inventory.ViewModels.RegradingResultDocs
{
    public class RegradingResultDocsReportViewModel
    {     
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }       
        public double TotalQuantity { get; set; }
        public double TotalLength { get; set; }
        public bool IsReturn { get; set; }
        public bool IsReturnedToPurchasing { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}
