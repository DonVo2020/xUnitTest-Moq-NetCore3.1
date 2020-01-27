using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class MaterialDistributionNoteDetail : StandardEntity, IValidatableObject
    {
        public int MaterialDistributionNoteItemId { get; set; }
        public int MaterialsRequestNoteItemId { get; set; }
        public string ProductionOrderId { get; set; }
        public string ProductionOrderNo { get; set; }
        public bool ProductionOrderIsCompleted { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Grade { get; set; }
        public double Quantity { get; set; }
        public double DistributedLength { get; set; }
        public double MaterialRequestNoteItemLength { get; set; }
        public double ReceivedLength { get; set; }
        public bool IsDisposition { get; set; }
        public bool IsCompleted { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public virtual MaterialDistributionNoteItem MaterialDistributionNoteItem { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
