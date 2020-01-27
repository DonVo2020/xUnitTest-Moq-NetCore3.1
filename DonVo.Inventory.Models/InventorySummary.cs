using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class InventorySummary : StandardEntity
    {
        public string No { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UomUnit { get; set; }
        public int UomId { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public double Quantity { get; set; }
        public double StockPlanning { get; set; }

        [MaxLength(255)]
        public string Uid { get; set; }
    }
}