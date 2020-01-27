using DonVo.Inventory.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels
{
    public class RegradingResultDetailsDocsViewModel : BasicViewModel, IValidatableObject
    {
        public double Quantity { get; set; }
        public double Length { get; set; }
        public double LengthBeforeReGrade { get; set; }
        public string Remark { get; set; }
        public string Grade { get; set; }
        public string GradeBefore { get; set; }
        public string Return { get; set; }
        public CodeNameViewModel Product { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
