using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class MaterialsRequestNoteItem : StandardEntity, IValidatableObject
    {
        public int MaterialsRequestNoteId { get; set; }
        public string Code { get; set; }
        public string ProductionOrderId { get; set; }
        public string ProductionOrderNo { get; set; }
        public bool ProductionOrderIsCompleted { get; set; }
        public double OrderQuantity { get; set; }
        public string OrderTypeId { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderTypeName { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Grade { get; set; }
        public double DistributedLength { get; set; }
        public double Length { get; set; }
        public string Remark { get; set; }

        public virtual MaterialsRequestNote MaterialsRequestNote { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
