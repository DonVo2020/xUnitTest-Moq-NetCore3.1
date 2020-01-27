using DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels;
using DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel;
using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.SampleData
{
    public class MaterialRequestNoteSampleData
    {
        private readonly MaterialRequestNoteRepository _materialRequestNoteRepository;

        public MaterialRequestNoteSampleData(MaterialRequestNoteRepository materialRequestNoteRepository)
        {
            _materialRequestNoteRepository = materialRequestNoteRepository;
        }

        public MaterialsRequestNoteViewModel GetEmptyData()
        {
            MaterialsRequestNoteViewModel data = new MaterialsRequestNoteViewModel
            {
                RequestType = string.Empty,
                Unit = new CodeNameViewModel(),
                Remark = string.Empty,
                MaterialsRequestNoteItems = new List<MaterialsRequestNoteItemViewModel> { new MaterialsRequestNoteItemViewModel() {
                Product = new CodeNameViewModel()
                {
                },
                ProductionOrder = new ProductionOrderViewModel()
                {
                    OrderQuantity = 0,
                    OrderType = new CodeNameViewModel()
                    {
                    }
                }
            } }
            };

            return data;
        }

        public MaterialsRequestNote GetNewData()
        {
            MaterialsRequestNote testData = new MaterialsRequestNote
            {
                UnitId = "1",
                UnitCode = "a",
                UnitName = "Name",
                Remark = "",
                RequestType = "AWAL",
                IsDistributed = false,
                IsCompleted = false,
                MaterialsRequestNoteItems = new List<MaterialsRequestNoteItem> { new MaterialsRequestNoteItem()
                {
                    Grade="a",
                    Length = 1,
                    OrderQuantity = 1,
                    OrderTypeCode = "Code",
                    OrderTypeId = "1",
                    OrderTypeName = "Name",
                    ProductCode = "Code",
                    ProductionOrderNo = "c",
                    Remark = "a",
                    ProductId = "1",
                    ProductionOrderId = "1",
                    ProductionOrderIsCompleted = true,
                    ProductName = "Name"
                }}
            };

            return testData;
        }

        public async Task<MaterialsRequestNote> GetTestData()
        {
            MaterialsRequestNote data = GetNewData();
            await _materialRequestNoteRepository.CreateAsync(data);
            return data;
        }
    }
}
