using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories.MaterialRequestNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.MaterialsRequestNoteViewModel;
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
    public class MaterialRequestNoteRepositoryTest
    {
        readonly Mock<IServiceProvider> serviceProvider;
        readonly Mock<IServiceProvider> getFailServiceProvider;
        readonly InventoryDbContext inventoryDbContext;
        readonly string getCurrentMethod;

        public MaterialRequestNoteRepositoryTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getFailServiceProvider = TestSetup.GetFailServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
        }

        private MaterialRequestNoteSampleData DataUtil(MaterialRequestNoteRepository repository)
        {
            return new MaterialRequestNoteSampleData(repository);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository).GetNewData();
            var response = await repository.CreateAsync(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository).GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.CreateAsync(data));
        }

        [Fact]
        public async Task Should_Success_DeleteAsync()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var response = await repository.DeleteAsync(data.Id);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_DeleteAsync()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.DeleteAsync(data.Id));
        }

        [Fact]
        public async Task Should_Fail_DeleteAsync_Null()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);

            await Assert.ThrowsAnyAsync<Exception>(() => repository.DeleteAsync(0));
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(sampleRepository).GetEmptyData();
            data.RequestType = "AWAL";

            var model = sampleRepository.MapToModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            var model = sampleRepository.MapToViewModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            var model = sampleRepository.Read(1, 25, "{}", null, null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            var model = await sampleRepository.ReadByIdAsync(data.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Update()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            var vm = sampleRepository.MapToViewModel(data);
            var testData = sampleRepository.MapToModel(vm);

            testData.MaterialsRequestNoteItems.FirstOrDefault().ProductionOrderId = "3";
            testData.MaterialsRequestNoteItems.Add(new MaterialsRequestNoteItem()
            {
                Grade = "a",
                Length = 1,
                OrderQuantity = 1,
                OrderTypeCode = "code",
                OrderTypeId = "1",
                OrderTypeName = "name",
                ProductCode = "code",
                ProductionOrderNo = "c",
                Remark = "a",
                ProductId = "1",
                ProductionOrderId = "1",
                ProductionOrderIsCompleted = true,
                ProductName = "name"
            });
            var response = await sampleRepository.UpdateAsync(testData.Id, testData);

            Assert.NotEqual(0, response);

            var newData = await sampleRepository.ReadByIdAsync(data.Id);
            var vm2 = sampleRepository.MapToViewModel(newData);
            var testData2 = sampleRepository.MapToModel(vm);
            testData2.MaterialsRequestNoteItems.Clear();
            var newresponse = await sampleRepository.UpdateAsync(testData2.Id, testData2);

            Assert.NotEqual(0, newresponse);
        }

        [Fact]
        public async Task Should_Fail_UpdateAsync()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateAsync(data.Id, data));
        }

        [Fact]
        public async Task Should_Fail_UpdateAsync_Null()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);

            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateAsync(0, new MaterialsRequestNote()));
        }

        [Fact]
        public async Task Should_Success_UpdateIsCompleted()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            await repository.UpdateIsCompleted(data.Id, data);
            Assert.True(true);
        }

        [Fact]
        public async Task Should_Success_UpdateIsCompleted_False()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            foreach (var item in data.MaterialsRequestNoteItems)
            {
                item.ProductionOrderIsCompleted = false;
            }
            await repository.UpdateIsCompleted(data.Id, data);
            Assert.True(true);
        }

        [Fact]
        public async Task Should_Fail_UpdateIsCompleted()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateIsCompleted(data.Id, data));
        }

        [Fact]
        public async Task Should_Success_UpdateDistributedQuantity()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            repository.UpdateDistributedQuantity(data.Id, data);
            Assert.True(true);
        }

        [Fact]
        public async Task Should_Fail_UpdateDistributedQuantity()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();
            Assert.ThrowsAny<Exception>(() => repository.UpdateDistributedQuantity(data.Id, data));
        }

        [Fact]
        public async Task Should_Success_GetReport()
        {
            MaterialRequestNoteRepository repository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var response = repository.GetReport(null, null, null, null, null, null, null, 1, 25, "{}", 6);
            Assert.True(true);
        }

        [Fact]
        public async Task Should_Success_ValidateVM()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(sampleRepository).GetTestData();

            var vm = sampleRepository.MapToViewModel(data);
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() == 0);


        }

        [Fact]
        public void Should_Success_ValidateNullVM()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new MaterialsRequestNoteViewModel();
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);


        }

        [Fact]
        public void Should_Success_ValidateNullDetailVM()
        {
            MaterialRequestNoteRepository sampleRepository = new MaterialRequestNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new MaterialsRequestNoteViewModel() { RequestType = "a", MaterialsRequestNoteItems = new List<MaterialsRequestNoteItemViewModel>() { new MaterialsRequestNoteItemViewModel() } };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);


        }
    }
}
