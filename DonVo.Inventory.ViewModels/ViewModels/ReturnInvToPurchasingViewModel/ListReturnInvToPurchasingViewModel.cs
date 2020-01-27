using System;

namespace DonVo.Inventory.ViewModels.FPReturnInvToPurchasingViewModel
{
    public class ListReturnInvToPurchasingViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string UnitName { get; set; }      
        public double TotalQuantity { get; set; }
        public double TotalLength { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
