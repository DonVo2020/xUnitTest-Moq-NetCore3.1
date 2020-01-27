using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel
{
    public class MaterialsRequestNoteItemViewModel : BasicViewModel, IValidatableObject
    {
       
        public double? Length { get; set; }
        public double? DistributedLength { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }
        public ProductionOrderViewModel ProductionOrder { get; set; }
        public CodeNameViewModel Product { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
