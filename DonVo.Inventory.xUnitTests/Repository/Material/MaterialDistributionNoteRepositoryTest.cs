using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Domains.Repositories.MaterialDistributionNoteRepositories;
using DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DonVo.Inventory.xUnitTests.Repository.Material
{
    public class MaterialDistributionNoteRepositoryTest
    {
        readonly Mock<IServiceProvider> serviceProvider;
        readonly Mock<IServiceProvider> getFailServiceProvider;
        readonly InventoryDbContext inventoryDbContext;
        readonly string getCurrentMethod;

        public MaterialDistributionNoteRepositoryTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getFailServiceProvider = TestSetup.GetFailServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
        }

        private MaterialDistributionNoteSampleData DataUtil(MaterialDistributionNoteRepository repository, MaterialRequestNoteRepository mrnRepository)
        {
            return new MaterialDistributionNoteSampleData(repository, mrnRepository);
        }

        private MaterialRequestNoteSampleData DataUtilMrn(MaterialRequestNoteRepository repository)
        {
            return new MaterialRequestNoteSampleData(repository);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository, repositoryMrn).GetNewData();

            foreach (var item in data.MaterialDistributionNoteItems)
            {
                item.MaterialRequestNoteId = mrn.Id;
                item.MaterialRequestNoteCreatedDateUtc = mrn.CreatedUtc;
                item.MaterialRequestNoteCode = mrn.Code;
            }
            var response = await repository.CreateAsync(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository, repositoryMrn).GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.CreateAsync(data));
        }

        [Fact]
        public async Task Should_Success_DeleteAsync()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var response = await repository.DeleteAsync(data.Id);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_DeleteAsync()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(getFailServiceProvider.Object, inventoryDbContext);

            await Assert.ThrowsAnyAsync<Exception>(() => repository.DeleteAsync(0));
        }

        //[Fact]
        //public async Task Should_Success_GetPdfReport()
        //{
        //    MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
        //    MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
        //    var data = await DataUtil(repository, repositoryMrn).GetTestData();
        //    var response = repository.GetPdfReport(null, null, null, DateTime.UtcNow, 7);
        //    Assert.NotEmpty(response);
        //}

        [Fact]
        public void Should_Success_GetReport()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository, repositoryMrn).GetTestData();
            var response = repository.GetReport(null, null, DateTime.UtcNow, 1, 25, "{}", 7);
            Assert.NotNull(response);
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository, repositoryMrn).GetEmptyData();


            var model = repository.MapToModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var model = repository.MapToViewModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var model = repository.Read(1, 25, "{}", null, null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var model = await repository.ReadByIdAsync(data.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Update()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var vm = repository.MapToViewModel(data);
            var testData = repository.MapToModel(vm);
            testData.MaterialDistributionNoteItems.Add(new MaterialDistributionNoteItem()
            {
                MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetail>()
                {
                    new MaterialDistributionNoteDetail()
                }
            });
            testData.UnitName = "a";

            var response = await repository.UpdateAsync(testData.Id, testData);

            Assert.NotEqual(0, response);

            var newData3 = await repository.ReadByIdAsync(data.Id);
            var vm3 = repository.MapToViewModel(newData3);
            var testData3 = repository.MapToModel(vm);
            var dItem = testData3.MaterialDistributionNoteItems.FirstOrDefault();
            dItem.MaterialDistributionNoteDetails = new List<MaterialDistributionNoteDetail>()
            {
                new MaterialDistributionNoteDetail()
                {

                }
            };
            var newResponse3 = await repository.UpdateAsync(testData3.Id, testData3);
            Assert.NotEqual(0, newResponse3);
            var newData = await repository.ReadByIdAsync(data.Id);
            var vm2 = repository.MapToViewModel(newData);
            var testData2 = repository.MapToModel(vm);
            testData2.MaterialDistributionNoteItems.Clear();
            var newResponse = await repository.UpdateAsync(testData2.Id, testData2);
            Assert.NotEqual(0, newResponse);
        }

        [Fact]
        public async Task Should_Fail_UpdateAsync()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(getFailServiceProvider.Object, inventoryDbContext);

            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateAsync(99, new MaterialDistributionNote()));
        }

        [Fact]
        public async Task Should_Success_UpdateIsApprove()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            var mrn = await DataUtilMrn(repositoryMrn).GetTestData();
            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();
            var response = repository.UpdateIsApprove(new List<int>() { data.Id });

            Assert.True(response);
        }

        [Fact]
        public async Task Should_Success_ValidateVM()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);
            await InventorySummaryRepository.Create(new InventorySummary()
            {
                StorageName = "Warehouse Here Finishing",
                UomUnit = "MTR",
                ProductCode = "Code",
                Quantity = 1
            });
            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);

            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository, repositoryMrn).GetTestData();

            var vm = repository.MapToViewModel(data);
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);

            var response = vm.Validate(InventorySummaryRepository.GetByStorageAndMTR);

            Assert.True(response.Count() == 0);
        }

        [Fact]
        public void Should_Success_ValidateNullVM()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new MaterialDistributionNoteViewModel() { MaterialDistributionNoteItems = new List<MaterialDistributionNoteItemViewModel>() };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(InventorySummaryRepository.GetByStorageAndMTR);

            Assert.True(response.Count() > 0);
        }

        [Fact]
        public void Should_Success_ValidateNullDetailVM()
        {
            MaterialRequestNoteRepository repositoryMrn = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            InventorySummaryRepository InventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(InventorySummaryRepository);

            InventoryMovementRepository InventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(InventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            serviceProvider.Setup(x => x.GetService(typeof(IMaterialRequestNoteRepository)))
                .Returns(repositoryMrn);

            MaterialDistributionNoteRepository repository = new MaterialDistributionNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new MaterialDistributionNoteViewModel() { MaterialDistributionNoteItems = new List<MaterialDistributionNoteItemViewModel>() { new MaterialDistributionNoteItemViewModel() } };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);

            var response = vm.Validate(InventorySummaryRepository.GetByStorageAndMTR);

            Assert.True(response.Count() > 0);
        }
    }
}
