using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class ReturnInvToPurchasingDetail : StandardEntity, IValidatableObject
    {
        public int ReturnInvToPurchasingId { get; set; }
        public int RegradingResultDocsId { get; set; }
        public string RegradingResultDocsCode { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double NecessaryLength { get; set; }
        public double Length { get; set; }
        public string Description { get; set; }
        public virtual ReturnInvToPurchasing ReturnInvToPurchasing { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
