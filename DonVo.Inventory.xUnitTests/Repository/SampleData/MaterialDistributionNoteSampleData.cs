using DonVo.Inventory.Domains.Repositories.MaterialDistributionNoteRepositories;
using DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels;
using DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class MaterialDistributionNoteSampleData
    {
        private readonly MaterialDistributionNoteRepository _repository;
        private readonly MaterialRequestNoteRepository _materialRequestNoteRepository;
        public MaterialDistributionNoteSampleData(MaterialDistributionNoteRepository repository, MaterialRequestNoteRepository materialRequestNoteRepository)
        {
            _repository = repository;
            _materialRequestNoteRepository = materialRequestNoteRepository;
        }

        public MaterialDistributionNoteViewModel GetEmptyData()
        {
            MaterialDistributionNoteViewModel Data = new MaterialDistributionNoteViewModel
            {
                Type = string.Empty,
                Unit = new CodeNameViewModel()
                {
                    Id = "1"
                },
                MaterialDistributionNoteItems = new List<MaterialDistributionNoteItemViewModel> {
                    new MaterialDistributionNoteItemViewModel {
                        MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetailViewModel>() { new MaterialDistributionNoteDetailViewModel()
                        {
                            Product = new CodeNameViewModel(),
                            ProductionOrder = new ProductionOrderViewModel(),
                            Supplier = new CodeNameViewModel()
                        }
                    }
                }
            }
            };

            return Data;
        }

        public MaterialDistributionNote GetNewData()
        {

            MaterialDistributionNote TestData = new MaterialDistributionNote
            {
                UnitId = "1",
                UnitCode = "Code",
                UnitName = "Name",
                Type = "PRODUKSI",
                IsApproved = false,
                IsDisposition = false,
                MaterialDistributionNoteItems = new List<MaterialDistributionNoteItem> {
                    new MaterialDistributionNoteItem()
                    {
                        MaterialRequestNoteCode = "Code",
                        MaterialRequestNoteCreatedDateUtc = DateTime.UtcNow,
                        MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetail>()
                        {
                            new MaterialDistributionNoteDetail()
                            {
                                DistributedLength = 1,
                                Grade = "a",
                                ProductCode = "Code",
                                ProductId = "1",
                                MaterialsRequestNoteItemId = 1,
                                MaterialRequestNoteItemLength = 1,
                                ProductionOrderId = "1",
                                ProductionOrderNo = "No",
                                ProductName = "Name",
                                SupplierCode = "Code",
                                SupplierId = "1",
                                SupplierName = "Name",
                                ReceivedLength = 1,
                                Quantity = 1
                            }
                        }
                    }
                }
            };

            return TestData;
        }


        public async Task<MaterialDistributionNote> GetTestData()
        {
            MaterialRequestNoteSampleData materialRequestNoteDataUtil = new MaterialRequestNoteSampleData(_materialRequestNoteRepository);
            var mrn = await materialRequestNoteDataUtil.GetTestData();

            MaterialDistributionNote Data = GetNewData();
            foreach (var item in Data.MaterialDistributionNoteItems)
            {
                item.MaterialRequestNoteId = mrn.Id;
                item.MaterialRequestNoteCreatedDateUtc = mrn.CreatedUtc;
                item.MaterialRequestNoteCode = mrn.Code;
                foreach (var detail in item.MaterialDistributionNoteDetails)
                {
                    detail.MaterialsRequestNoteItemId = mrn.Id;
                }
            }
            await _repository.CreateAsync(Data);
            return Data;
        }
    }
}
