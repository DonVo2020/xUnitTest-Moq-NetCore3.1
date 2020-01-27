using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.InventoryViewModel
{
    public class InventoryDocumentItemViewModel : BasicViewModel, IValidatableObject
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double StockPlanning { get; set; }
        public int UomId { get; set; }
        public string Uom { get; set; }
        public string Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}