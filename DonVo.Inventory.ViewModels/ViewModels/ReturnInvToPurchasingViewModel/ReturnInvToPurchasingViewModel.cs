using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.FPReturnInvToPurchasingViewModel
{
    public class ReturnInvToPurchasingViewModel : BasicViewModel, IValidatableObject
    {
        public string No { get; set; }
        public int AutoIncrementNumber { get; set; }

        public CodeNameViewModel Unit { get; set; }
        public CodeNameViewModel Supplier { get; set; }       
        public List<ReturnInvToPurchasingDetailViewModel> ReturnInvToPurchasingDetails { get; set; }

        public ReturnInvToPurchasingViewModel() { }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int Count = 0;

            if (this.Unit == null || string.IsNullOrWhiteSpace(this.Unit.Name))
                yield return new ValidationResult("Unit is required", new List<string> { "Unit" });

            if (this.Supplier == null || string.IsNullOrWhiteSpace(this.Supplier.Id))
                yield return new ValidationResult("Supplier is required", new List<string> { "Supplier" });

            if (ReturnInvToPurchasingDetails.Count.Equals(0))
            {
                yield return new ValidationResult("Details is required", new List<string> { "ReturnInvToPurchasingCollection" });
            }
            else
            {
                string DetailError = "[";

                foreach (ReturnInvToPurchasingDetailViewModel detail in this.ReturnInvToPurchasingDetails)
                {
                    if (string.IsNullOrWhiteSpace(detail.RegradingResultDocsCode))
                    {
                        Count++;
                        DetailError += "{ RegradingResultDocs: 'Regrading Result Docs No is required' }, ";
                    }

                    else if (detail.NecessaryLength <= 0)
                    {
                        Count++;
                        DetailError += "{ NecessaryLength: 'Necessary Length must be greater than zero' }, ";
                    }
                }

                if (Count > 0)
                {
                    yield return new ValidationResult(DetailError, new List<string> { "ReturnInvToPurchasingDetails" });
                }
            }
        }
    }
}
