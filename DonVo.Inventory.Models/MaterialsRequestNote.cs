using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class MaterialsRequestNote : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string Remark { get; set; }
        public string RequestType { get; set; }
        public string Type { get; set; }
        public bool IsDistributed { get; set; }
        public bool IsCompleted { get; set; }
        public int AutoIncrementNumber { get; set; }
        public virtual ICollection<MaterialsRequestNoteItem> MaterialsRequestNoteItems { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
