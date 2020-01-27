using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel
{
    public class MaterialsRequestNoteViewModel : BasicViewModel, IValidatableObject
    {
        public int AutoIncrementNumber { get; set; }
        public string Code { get; set; }      
        public string RequestType { get; set; }
        public string Remark { get; set; }
        public bool IsDistributed { get; set; }
        public bool IsCompleted { get; set; }
        public CodeNameViewModel Unit { get; set; }
        public List<MaterialsRequestNoteItemViewModel> MaterialsRequestNoteItems { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Unit == null || string.IsNullOrWhiteSpace(this.Unit.Id))
                yield return new ValidationResult("Unit must be filled", new List<string> { "UnitId" });
            if (string.IsNullOrWhiteSpace(this.RequestType))
                yield return new ValidationResult("RequestType Request must be filled", new List<string> { "RequestType" });

            int Count = 0;
            string materialsRequestNoteItemsError = "[";

            if (this.MaterialsRequestNoteItems == null || this.MaterialsRequestNoteItems.Count.Equals(0))
                yield return new ValidationResult("The table below must be filled", new List<string> { "MaterialsRequestNoteItems" });
            else
            {
                foreach (MaterialsRequestNoteItemViewModel materialsRequestNoteItem in this.MaterialsRequestNoteItems)
                {
                    materialsRequestNoteItemsError += "{";
                    if (materialsRequestNoteItem.Product == null || string.IsNullOrWhiteSpace(materialsRequestNoteItem.Product.Id))
                    {
                        Count++;
                        materialsRequestNoteItemsError += "ProductId: 'Item must be filled', ";
                    }
                    else if (materialsRequestNoteItem.Product.Name != null || string.IsNullOrWhiteSpace(materialsRequestNoteItem.Product.Name))
                    {
                        //string inventorySummaryURI = "inventory/inventory-summary?order=%7B%7D&page=1&size=1000000000&";
                        var StorageName = this.Unit.Name.Equals("PRINTING") ? "Warehouse Here Printing" : "Warehouse Here Finishing";
                        //MaterialsRequestNoteService Service = (MaterialsRequestNoteService)validationContext.GetService(typeof(MaterialsRequestNoteService));
                        //InventorySummaryFacade InventorySummaryFacade = (InventorySummaryFacade)validationContext.GetService(typeof(InventorySummaryFacade));
                        //List<string> inventorySummaries = InventorySummaryFacade.GetProductCodeForMaterialRequestNote(storageName);

                        //HttpClient httpClient = new HttpClient();
                        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.Token);

                        List<string> products = new List<string>();
                        foreach (MaterialsRequestNoteItemViewModel item in this.MaterialsRequestNoteItems)
                        {

                            products.Add(item.Product.Code);
                        }

                        Dictionary<string, object> filter = new Dictionary<string, object> { { "StorageName", StorageName }, { "Uom", "MTR" }, { "ProductCode", new Dictionary<string, object> { { "$in", products.ToArray() } } } };
                    }
                    if (string.IsNullOrWhiteSpace(materialsRequestNoteItem.Grade))
                    {
                        Count++;
                        materialsRequestNoteItemsError += "Grade: 'Grade must be filled', ";
                    }
                    if (materialsRequestNoteItem.Length == null)
                    {
                        Count++;
                        materialsRequestNoteItemsError += "Length: 'Length must be filled', ";
                    }
                    else if (materialsRequestNoteItem.Length <= 0)
                    {
                        Count++;
                        materialsRequestNoteItemsError += "Length: 'Length must be greater than 0', ";
                    }

                    if (!string.Equals(this.RequestType.ToUpper(), "TEST") && !string.Equals(this.RequestType.ToUpper(), "PURCHASE"))
                    {
                        if (materialsRequestNoteItem.ProductionOrder != null && materialsRequestNoteItem.Length > (materialsRequestNoteItem.ProductionOrder.OrderQuantity * 1.05))
                        {
                            Count++;
                            materialsRequestNoteItemsError += "Length: 'Length total cannot be more than 105% tolerance', ";
                        }
                        if (materialsRequestNoteItem.ProductionOrder == null || string.IsNullOrWhiteSpace(materialsRequestNoteItem.ProductionOrder.Id))
                        {
                            Count++;
                            materialsRequestNoteItemsError += "ProductionOrderId: 'Production Warrants must be filled', ";
                        }
                        if (materialsRequestNoteItem.ProductionOrder != null)
                        {
                            int count = MaterialsRequestNoteItems
                                .Count(c => string.Equals(c.ProductionOrder.Id, materialsRequestNoteItem.ProductionOrder.Id));

                            if (count > 1)
                            {
                                Count++;
                                materialsRequestNoteItemsError += "ProductionOrderId: 'Production Order Letter must not be duplicated', ";
                            }
                        }
                    }
                    materialsRequestNoteItemsError += "}, ";
                }
            }
            materialsRequestNoteItemsError += "]";
            if (Count > 0)
            {
                yield return new ValidationResult(materialsRequestNoteItemsError, new List<string> { "MaterialsRequestNoteItems" });
            }
        }
    }
}
