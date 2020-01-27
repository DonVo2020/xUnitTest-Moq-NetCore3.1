using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class InventoryDocumentItem : StandardEntity, IValidatableObject
    {
        /* Product */
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UomUnit { get; set; }
        public int UomId { get; set; }
        public double Quantity { get; set; }
        public double StockPlanning { get; set; }
        public string ProductRemark { get; set; }
        public int MongoIndexItem { get; set; }
        public int InventoryDocumentId { get; set; }
        public virtual InventoryDocument InventoryDocument { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}

