using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class StockTransferNoteItem : StandardEntity, IValidatableObject
    {
        public int StockTransferNoteId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double StockQuantity { get; set; }
        public string UomUnit { get; set; }
        public string UomId { get; set; }
        public double TransferedQuantity { get; set; }
        public virtual StockTransferNote StockTransferNote { get; set; }
        public IEnumerable<StockTransferNoteItem> MaterialDistributionNoteDetails { get; internal set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
