using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class RegradingResultDocs : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string NoBon { get; set; } 
        public string NoBonId { get; set; }
        public string UnitName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Remark { get; set; }
        public string Operator { get; set; }
        public string MachineName { get; set; }
        public string MachineId { get; set; }
        public string MachineCode { get; set; }
        public string Shift { get; set; }
        public double TotalLength { get; set; }
        public string OriginalGrade { get; set; }
        public bool IsReturn { get; set; }
        public bool IsReturnedToPurchasing { get; set; }
        public int AutoIncrementNumber { get; set; }
        public DateTimeOffset Date { get; set; }
        public ICollection<RegradingResultDocsDetails> Details { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
