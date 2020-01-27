using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class ReturnInvToPurchasing : StandardEntity, IValidatableObject
    {
        public string No { get; set; }
        public string UnitName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public int AutoIncrementNumber { get; set; }
        public virtual ICollection<ReturnInvToPurchasingDetail> ReturnInvToPurchasingDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
