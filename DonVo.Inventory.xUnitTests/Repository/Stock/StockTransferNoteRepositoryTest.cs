using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Domains.Repositories.StockTransferNoteRepositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.StockTransferNoteViewModel;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DonVo.Inventory.xUnitTests.Repository.Stock
{
    public class StockTransferNoteRepositoryTest
    {
        readonly Mock<IServiceProvider> serviceProvider;
        readonly Mock<IServiceProvider> getFailServiceProvider;
        readonly InventoryDbContext inventoryDbContext;
        readonly string getCurrentMethod;

        public StockTransferNoteRepositoryTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getFailServiceProvider = TestSetup.GetFailServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
        }

        private StockTransferNoteSampleData DataUtil(StockTransferNoteRepository repository)
        {
            return new StockTransferNoteSampleData(repository);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository).GetNewData();

            var response = await repository.CreateAsync(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            StockTransferNoteRepository repository = new StockTransferNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository).GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.CreateAsync(data));
        }

        [Fact]
        public async Task Should_Success_DeleteAsync()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();

            var response = await repository.DeleteAsync(data.Id);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_DeleteAsync()
        {
            StockTransferNoteRepository repository = new StockTransferNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            await Assert.ThrowsAnyAsync<Exception>(() => repository.DeleteAsync(0));
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            StockTransferNoteRepository repository = new StockTransferNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            var data = DataUtil(repository).GetNewData();
            var vm = repository.MapToViewModel(data);

            var model = repository.MapToModel(vm);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var model = repository.MapToViewModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var model = repository.Read(1, 25, "{}", null, null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var model = await repository.ReadByIdAsync(data.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Update()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var vm = repository.MapToViewModel(data);
            var testData = repository.MapToModel(vm);
            testData.StockTransferNoteItems.Add(new StockTransferNoteItem
            {
                ProductCode = "code",
                ProductId = "1",
                ProductName = "name",
                StockQuantity = 2,
                TransferedQuantity = 2,
                UomId = "2",
                UomUnit = "unitA"
            });
            testData.TargetStorageName = "a";

            var response = await repository.UpdateAsync(testData.Id, testData);

            Assert.NotEqual(0, response);

            var newData = await repository.ReadByIdAsync(data.Id);
            var vm2 = repository.MapToViewModel(newData);
            var testData2 = repository.MapToModel(vm2);
            testData2.StockTransferNoteItems.Clear();
            var newResponse = await repository.UpdateAsync(testData2.Id, testData2);

            Assert.NotEqual(0, newResponse);
        }

        [Fact]
        public async Task Should_Fail_UpdateAsync()
        {
            StockTransferNoteRepository repository = new StockTransferNoteRepository(getFailServiceProvider.Object, inventoryDbContext);
            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateAsync(99, new StockTransferNote()));
        }

        [Fact]
        public async Task Should_Success_ReadModelByNotUser()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();
            var model = repository.ReadModelByNotUser(1, 25, "{}", null, null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_UpdateIsApproved()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();

            var response = await repository.UpdateIsApprove(data.Id);

            Assert.True(response);
        }

        [Fact]
        public async Task Should_Fail_UpdateIsApproved()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();

            await Assert.ThrowsAnyAsync<Exception>(() => repository.UpdateIsApprove(99));
        }

        [Fact]
        public async Task Should_Success_ValidateVM()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var data = await DataUtil(repository).GetTestData();

            var vm = repository.MapToViewModel(data);
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() == 0);
        }

        [Fact]
        public void Should_Success_ValidateNullVM()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new StockTransferNoteViewModel();
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);
        }

        [Fact]
        public void Should_Success_ValidateNullDetailVM()
        {
            InventorySummaryRepository inventorySummaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(inventorySummaryRepository);

            InventoryMovementRepository inventoryMovementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(inventoryMovementRepository);

            InventoryDocumentRepository inventoryDocumentRepository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryDocumentRepository)))
                .Returns(inventoryDocumentRepository);

            StockTransferNoteRepository repository = new StockTransferNoteRepository(serviceProvider.Object, inventoryDbContext);
            var vm = new StockTransferNoteViewModel() { StockTransferNoteItems = new List<StockTransferNoteItemViewModel>() { new StockTransferNoteItemViewModel() } };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);
        }
    }
}
