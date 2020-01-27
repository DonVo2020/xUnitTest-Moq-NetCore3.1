using DonVo.Inventory.ViewModels.ReturnFromBuyers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.FpReturnFromBuyers
{
    public class ReturnFromBuyerViewModel : BasicViewModel, IValidatableObject
    {        
        public string Code { get; set; }
        public string CodeProduct { get; set; }
        public string CoverLetter { get; set; }      
        public string Destination { get; set; }     
        public string SpkNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public IList<ReturnFromBuyerDetailViewModel> Details { get; set; }
        public StorageIntegrationViewModel Storage { get; set; }
        public BuyerViewModel Buyer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null || Date.Date > DateTime.Now.Date)
                yield return new ValidationResult("Date Required or not valid", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(Destination))
                yield return new ValidationResult("Destination receive must be filled in", new List<string> { "Destination" });

            if (Buyer == null || Buyer.Id < 1)
                yield return new ValidationResult("Buyer invalid or required", new List<string> { "Buyer" });

            if (Storage == null || Storage._id < 1)
                yield return new ValidationResult("Warehouse invalid or required", new List<string> { "Storage" });

            int detailErrorCount = 0;
            string detailsError = "[";
            if (Details == null || Details.Count == 0)
                yield return new ValidationResult("Detail Production Warrants must be filled in at a minimum 1", new List<string> { "Detail" });
            else
            {
                foreach (var detail in Details)
                {
                    detailsError += "{";

                    if (detail.ProductionOrder == null || detail.ProductionOrder.Id < 1)
                    {
                        detailErrorCount++;
                        detailsError += "ProductionOrder: 'Production Warrants must be filled', ";
                    }

                    string itemsError = "[";
                    if (detail.Items == null || detail.Items.Count < 1)
                    {
                        detailErrorCount++;
                        detailsError += "Item: 'Item Item must be filled minimal 1', ";
                    }
                    else
                    {
                        detailErrorCount++;
                        detailsError += "Items: [";

                        foreach (var item in detail.Items)
                        {
                            itemsError += "{";

                            if (item.ProductId < 1)
                                itemsError += "ProductId: 'Item must be filled'";

                            if (item.ReturnQuantity <= 0)
                                itemsError += "ReturnQuantity: 'Amount returns must be more than 0'";

                            if (item.Length <= 0)
                                itemsError += "Length: 'Length must be more than 0'";

                            if (item.Weight <= 0)
                                itemsError += "Weight: 'Weight must be more than 0'";

                            itemsError += "}, ";
                        }

                        detailsError += itemsError;
                        detailsError += "], ";
                    }

                    detailsError += "},";
                }
                detailsError += "]";
            }

            if (detailErrorCount > 0)
            {
                yield return new ValidationResult(detailsError, new List<string> { "Details" });
            }
        }
    }

    public class BuyerViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class StorageIntegrationViewModel
    {
        public int _id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
