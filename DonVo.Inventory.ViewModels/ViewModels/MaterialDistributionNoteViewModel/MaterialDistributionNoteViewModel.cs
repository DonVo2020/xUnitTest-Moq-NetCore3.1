using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel
{
    public class MaterialDistributionNoteViewModel : BasicViewModel
    {
        public string No { get; set; }       
        public string Type { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDisposition { get; set; }
        public CodeNameViewModel Unit { get; set; }
        public List<MaterialDistributionNoteItemViewModel> MaterialDistributionNoteItems { get; set; }

        public IEnumerable<ValidationResult> Validate(Func<string,List<InventorySummary>> inventorySummaries)
        {
            int Count = 0;

            if (this.Unit == null || string.IsNullOrWhiteSpace(this.Unit.Id))
                yield return new ValidationResult("Unit is required", new List<string> { "Unit" });

            if (string.IsNullOrWhiteSpace(this.Type))
                yield return new ValidationResult("Type is required", new List<string> { "Type" });

            if (MaterialDistributionNoteItems.Count.Equals(0))
            {
                yield return new ValidationResult("Material Distribution Note Item is required", new List<string> { "MaterialDistributionNoteItemCollection" });
            }
            else
            {
                string materialDistributionNoteItemError = "[";

                foreach (MaterialDistributionNoteItemViewModel mdni in this.MaterialDistributionNoteItems)
                {
                    if (string.IsNullOrWhiteSpace(mdni.MaterialRequestNoteCode))
                    {
                        Count++;
                        materialDistributionNoteItemError += "{ MaterialRequestNote: 'SPB is required' }, ";
                    }
                }

                if (Count.Equals(0))
                {
                    /* Get Inventory Summaries */
                    //string inventorySummaryURI = "inventory/inventory-summary?order=%7B%7D&page=1&size=1000000000&";

                    //MaterialDistributionNoteService Service = (MaterialDistributionNoteService)validationContext.GetService(typeof(MaterialDistributionNoteService));
                    //HttpClient httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.Token);
                    //var inventorySummaryFacade = validationContext.GetService<IInventorySummaryService>();
                    //var inventoryRepositoryFacade = validationContext.GetService<IInventorySummaryRepository>();


                    List<string> products = new List<string>();
                    foreach (MaterialDistributionNoteItemViewModel mdni in this.MaterialDistributionNoteItems)
                    {
                        products.AddRange(mdni.MaterialDistributionNoteDetails.Select(p => p.Product.Code).ToList());
                    }

                    var storageName = this.Unit.Name.Equals("PRINTING") ? "Warehouse Here Printing" : "Warehouse Here Finishing";
                    var tempInventorySummaries = inventorySummaries(storageName);
                    //List<InventorySummary> inventorySummaries = inventoryRepositoryFacade.GetByStorageAndMTR(storageName);
                    //Dictionary<string, object> filter = new Dictionary<string, object> { { "StorageName", StorageName }, { "uom", "MTR" }, { "productCode", new Dictionary<string, object> { { "$in", products.ToArray() } } } };
                    //var response = httpClient.GetAsync($@"{APIEndpoint.Inventory}{inventorySummaryURI}filter=" + JsonConvert.SerializeObject(filter)).Result.Content.ReadAsStringAsync();
                    //Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);

                    //var json = result.Single(p => p.Key.Equals("data")).Value;
                    //List<InventorySummaryViewModel> inventorySummaries = JsonConvert.DeserializeObject<List<InventorySummaryViewModel>>(json.ToString());

                    foreach (MaterialDistributionNoteItemViewModel mdni in this.MaterialDistributionNoteItems)
                    {
                        if (mdni.MaterialDistributionNoteDetails.Count(p => p.ReceivedLength != null && p.ReceivedLength > 0).Equals(0))
                        {
                            Count++;
                            materialDistributionNoteItemError += "{ MaterialRequestNote: 'SPB detail is required' }, ";
                        }
                        else
                        {
                            int CountDetail = 0;

                            string materialDistributionNoteDetailError = "[";

                            foreach (MaterialDistributionNoteDetailViewModel mdnd in mdni.MaterialDistributionNoteDetails)
                            {
                                if (mdnd.ReceivedLength == null || mdnd.ReceivedLength == 0)
                                {
                                    materialDistributionNoteDetailError += "{}, ";
                                    continue;
                                }

                                var inventorySummary = tempInventorySummaries.FirstOrDefault(p => p.ProductCode.Equals(mdnd.Product.Code));

                                materialDistributionNoteDetailError += "{";

                                if (inventorySummary == null)
                                {
                                    CountDetail++;
                                    materialDistributionNoteDetailError += "Product: 'Product is not exists in the storage', ";
                                }
                                else
                                {
                                    if (mdnd.Quantity == null)
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "Quantity: 'Quantity is required', ";
                                    }
                                    else if (mdnd.Quantity <= 0)
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "Quantity: 'Quantity must be greater than zero', ";
                                    }

                                    if (mdnd.ReceivedLength == null)
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "ReceivedLength: 'Length is required', ";
                                    }
                                    else if (mdnd.ReceivedLength <= 0)
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "ReceivedLength: 'Length must be greater than zero', ";
                                    }
                                    else if (mdnd.ReceivedLength > inventorySummary.Quantity)
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "ReceivedLength: 'Length must be less than or equal than stock', ";
                                    }
                                    else
                                    {
                                        inventorySummary.Quantity -= (double)mdnd.ReceivedLength;
                                    }

                                    if (mdnd.Supplier == null || string.IsNullOrWhiteSpace(mdnd.Supplier.Id))
                                    {
                                        CountDetail++;
                                        materialDistributionNoteDetailError += "Supplier: 'Supplier is required', ";
                                    }
                                }

                                materialDistributionNoteDetailError += "}, ";
                            }

                            mdni.MaterialDistributionNoteDetails = mdni.MaterialDistributionNoteDetails.Where(p => p.ReceivedLength != null && p.ReceivedLength > 0).ToList();

                            materialDistributionNoteDetailError += "]";

                            if (CountDetail > 0)
                            {
                                Count++;
                                materialDistributionNoteItemError += string.Concat("{ MaterialDistributionNoteDetails: ", materialDistributionNoteDetailError, " }, ");
                            }
                            else
                            {
                                materialDistributionNoteItemError += "{}, ";
                            }
                        }
                    }
                }

                materialDistributionNoteItemError += "]";

                if (Count > 0)
                {
                    yield return new ValidationResult(materialDistributionNoteItemError, new List<string> { "MaterialDistributionNoteItems" });
                }
            }
        }
    }
}
