using System;

namespace DonVo.Inventory.ViewModels.InventoryViewModel
{
    public class TViewModel : BasicViewModel
    {
        public string No { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UomUnit { get; set; }
        public int UomId { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public double StockPlanning { get; set; }
        public double Before { get; set; }
        public double Quantity { get; set; }
        public double After { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
