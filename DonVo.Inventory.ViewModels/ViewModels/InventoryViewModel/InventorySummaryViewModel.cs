namespace DonVo.Inventory.ViewModels.InventoryViewModel
{
    public class InventorySummaryViewModel : BasicViewModel
    {
        public string Code { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public double Quantity { get; set; }
        public int UomId { get; set; }
        public string Uom { get; set; }
        public string StockPlanning { get; set; }
    }
}