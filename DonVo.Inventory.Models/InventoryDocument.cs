using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class InventoryDocument : StandardEntity, IValidatableObject
    {
        public string No { get; set; }      
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public DateTimeOffset Date { get; set; }
        public virtual ICollection<InventoryDocumentItem> Items { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
