using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class MaterialDistributionNoteItem : StandardEntity, IValidatableObject
    {
        public int MaterialDistributionNoteId { get; set; }
        public int MaterialRequestNoteId { get; set; }
        public string MaterialRequestNoteCode { get; set; }
        public DateTime MaterialRequestNoteCreatedDateUtc { get; set; }
        public virtual ICollection<MaterialDistributionNoteDetail> MaterialDistributionNoteDetails { get; set; }
        public virtual MaterialDistributionNote MaterialDistributionNote { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {        
            return new List<ValidationResult>();
        }
    }
}
