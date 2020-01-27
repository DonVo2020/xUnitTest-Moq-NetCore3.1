using DonVo.Inventory.ViewModels.FpReturnFromBuyers;
using DonVo.Inventory.ViewModels.ViewModels.ReturnFromBuyers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.ReturnFromBuyers
{
    public class ReturnFromBuyerDetailViewModel : BasicViewModel, IValidatableObject
    {
        public IList<ReturnFromBuyerItemViewModel> Items { get; set; }
        public ProductionOrderIntegrationViewModel ProductionOrder { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
