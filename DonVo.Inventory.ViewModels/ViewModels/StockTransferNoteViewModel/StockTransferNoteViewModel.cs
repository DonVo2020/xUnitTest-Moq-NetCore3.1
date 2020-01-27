using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DonVo.Inventory.ViewModels.StockTransferNoteViewModel
{
    public class StockTransferNoteViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public bool IsApproved { get; set; }
        public CodeNameViewModel SourceStorage { get; set; }
        public CodeNameViewModel TargetStorage { get; set; }        
        public List<StockTransferNoteItemViewModel> StockTransferNoteItems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int Count = 0;

            if (string.IsNullOrWhiteSpace(this.ReferenceNo))
                yield return new ValidationResult("No. reference must be filled in", new List<string> { "ReferenceNo" });

            if (string.IsNullOrWhiteSpace(this.ReferenceType))
                yield return new ValidationResult("RequestType reference must be filled in", new List<string> { "ReferenceType" });

            if (this.SourceStorage == null || string.IsNullOrWhiteSpace(this.SourceStorage.Id))
                yield return new ValidationResult("Warehouse asal must be filled", new List<string> { "SourceStorageId" });

            if (this.TargetStorage == null || string.IsNullOrWhiteSpace(this.TargetStorage.Id))
                yield return new ValidationResult("Warehouse tujuan must be filled", new List<string> { "TargetStorageId" });

            if (this.StockTransferNoteItems == null || this.StockTransferNoteItems.Count.Equals(0))
            {
                yield return new ValidationResult("Item must be filled", new List<string> { "StockTransferNoteItems" });
            }
            else
            {
                string stockTransferNoteItemError = "[";

                foreach (StockTransferNoteItemViewModel stockTransferNoteItem in this.StockTransferNoteItems)
                {
                    stockTransferNoteItemError += "{ ";
                    if (stockTransferNoteItem.Summary == null || stockTransferNoteItem.Summary.ProductId == 0)
                    {
                        Count++;
                        stockTransferNoteItemError += "SummaryId: 'Item must be filled', ";
                    }
                    else
                    {
                        int count = StockTransferNoteItems.Count(c => string.Equals(c.Summary.ProductName, stockTransferNoteItem.Summary.ProductName));

                        if (count > 1)
                        {
                            Count++;
                            stockTransferNoteItemError += "SummaryId: 'Items cannot be duplicated', ";
                        }
                    }

                    if (stockTransferNoteItem.TransferedQuantity == null || stockTransferNoteItem.TransferedQuantity <= 0)
                    {
                        Count++;
                        stockTransferNoteItemError += "TransferedQuantity: 'Quantity Transfer must be greater than 0', ";
                    }
                    else if (stockTransferNoteItem.TransferedQuantity > stockTransferNoteItem.Summary.Quantity)
                    {
                        Count++;
                        stockTransferNoteItemError += "TransferedQuantity : 'Quantity Transfer must be less than or equal to Quantity Stock', ";
                    }
                    stockTransferNoteItemError += "}, ";
                }

                stockTransferNoteItemError += "]";

                if (Count > 0)
                {
                    yield return new ValidationResult(stockTransferNoteItemError, new List<string> { "StockTransferNoteItems" });
                }
            }
        }
    }
}
