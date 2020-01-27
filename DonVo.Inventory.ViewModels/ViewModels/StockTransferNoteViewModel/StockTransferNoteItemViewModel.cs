using DonVo.Inventory.ViewModels.InventoryViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.StockTransferNoteViewModel
{
    public class StockTransferNoteItemViewModel : BasicViewModel, IValidatableObject
    {
        public int StockTransferNoteId { get; set; }
        public double? TransferedQuantity { get; set; }
        public InventorySummaryViewModel Summary { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
