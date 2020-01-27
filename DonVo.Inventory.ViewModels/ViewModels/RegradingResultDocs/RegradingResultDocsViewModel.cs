using DonVo.Inventory.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels
{
    public class RegradingResultDocsViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Operator { get; set; }
        public string Remark { get; set; }
        public string Shift { get; set; }
        public double TotalLength { get; set; }
        public string OriginalGrade { get; set; }
        public bool IsReturn { get; set; }
        public bool IsReturnedToPurchasing { get; set; }
        public List<RegradingResultDetailsDocsViewModel> Details { get; set; }
        public DateTimeOffset? Date { get; set; }
        public CodeNameViewModel Bon { get; set; }
        public CodeNameViewModel Supplier { get; set; }
        public CodeNameViewModel Product { get; set; }      
        public CodeNameViewModel Machine { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Bon == null || (this.Bon.Id) == "")
                yield return new ValidationResult("Bon must be filled", new List<string> { "Bon" });
            if (this.Machine == null || (this.Machine.Id) == "")
                yield return new ValidationResult("Mesin must be filled", new List<string> { "Machine" });
            if (this.Operator == null || string.IsNullOrWhiteSpace(this.Operator))
                yield return new ValidationResult("Operator must be filled", new List<string> { "Operator" });
            if (this.Date == null)
                yield return new ValidationResult("Date must be filled", new List<string> { "Date" });

            int Count = 0;
            string detailsError = "[";
            if (this.Details.Count.Equals(0) || this.Details == null)
            {
                yield return new ValidationResult("Details is required", new List<string> { "Details" });
            }
            else
            {
                //List<string> temp = new List<string>();
                foreach (RegradingResultDetailsDocsViewModel data in this.Details)
                {
                    detailsError += "{";
                    if (data.Length.Equals(0))
                    {
                        Count++;
                        detailsError += "Length: 'Length must be filled',";
                    }

                    if (data.Quantity.Equals(0))
                    {
                        Count++;
                        detailsError += "Quantity: 'Amount must be filled',";
                    }
                    detailsError += "},";
                }

                detailsError += "]";
            }
            if (Count > 0)
            {
                yield return new ValidationResult(detailsError, new List<string> { "Details" });
            }
        }
    }
}
