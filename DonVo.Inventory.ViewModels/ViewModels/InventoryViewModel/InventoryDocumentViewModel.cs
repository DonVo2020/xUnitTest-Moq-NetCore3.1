using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.InventoryViewModel
{
    public class InventoryDocumentViewModel : BasicViewModel, IValidatableObject
    {
        public string No { get; set; }
        public string Code { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public string Type { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public string Remark { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<InventoryDocumentItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StorageId==0)
                yield return new ValidationResult("Warehouse must be filled", new List<string> { "StorageId" });
            if (this.ReferenceNo == null || string.IsNullOrWhiteSpace(this.ReferenceNo))
                yield return new ValidationResult("No. reference must be filled in", new List<string> { "ReferenceNo" });
            if (this.ReferenceType == null || string.IsNullOrWhiteSpace(this.ReferenceType))
                yield return new ValidationResult("No. reference must be filled in", new List<string> { "ReferenceType" });
            if (this.Date.Equals(DateTimeOffset.MinValue) || this.Date == null)
            {
                yield return new ValidationResult("Date Required", new List<string> { "Date" });
            }
            int itemErrorCount = 0;

            if (this.Items.Count.Equals(0))
            {
                yield return new ValidationResult("Item Table Required", new List<string> { "ItemsCount" });
            }
            else
            {
                string itemError = "[";

                foreach (InventoryDocumentItemViewModel item in Items)
                {
                    itemError += "{";

                    if (item.ProductId==0)
                    {
                        itemErrorCount++;
                        itemError += "ProductId: 'Item must be filled', ";
                    }

                    if (Type=="ADJ" && item.Quantity == 0)
                    {
                        itemErrorCount++;
                        itemError += "Quantity: 'The amount of adjustments cannot be = 0',";
                    }
                    if (Type != "ADJ" && item.Quantity <= 0)
                    {
                        itemErrorCount++;
                        itemError += "Quantity: 'The number must be more than 0',";
                    }
                    if (item.Uom == null || item.UomId==0)
                    {
                        itemErrorCount++;
                        itemError += "UomId: 'The unit must be filled in', ";
                    }

                    itemError += "}, ";
                }

                itemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(itemError, new List<string> { "Items" });
            }
        }
    }
}
