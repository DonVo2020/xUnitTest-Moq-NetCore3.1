using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.Models;
using DonVo.Inventory.ViewModels.InventoryViewModel;
using DonVo.Inventory.xUnitTests.Repository;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DonVo.Year2020.Service.Inventory.Test.Facades.Inventory
{
    public class InventoryBasicTest
    {
        readonly Mock<IServiceProvider> serviceProvider;
        readonly InventoryDbContext inventoryDbContext;
        readonly InventoryDocumentSampleData documentSampleData;
        readonly InventoryDocumentRepository repository;
        readonly InventoryMovementRepository movementRepository;
        readonly InventorySummaryRepository summaryRepository;

        readonly string getCurrentMethod;

        public InventoryBasicTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
            repository = new InventoryDocumentRepository(serviceProvider.Object, inventoryDbContext);
            documentSampleData = TestSetup.DocumentSampleData(repository);
            movementRepository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            summaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = documentSampleData.GetNewData();

            var Response = await repository.Create(data);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = documentSampleData.GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.Create(null));
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = documentSampleData.GetNewDataViewModel();
            var model = repository.MapToModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = await documentSampleData.GetTestData();
            var model = repository.MapToViewModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = await documentSampleData.GetTestData();
            var model = repository.Read(1, 25, "{}", null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = await documentSampleData.GetTestData();
            var model = repository.ReadModelById(data.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_CreateMultiAsync()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = documentSampleData.GetNewData();

            var Response = await repository.CreateMulti(new List<InventoryDocument>() { data });
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Fail_CreateMultiAsync()
        {
            var data = documentSampleData.GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.CreateMulti(new List<InventoryDocument>() { new InventoryDocument() }));
        }

        [Fact]
        public async Task Should_Success_ValidateVM()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var data = await documentSampleData.GetTestData();

            var vm = repository.MapToViewModel(data);
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() == 0);
        }

        [Fact]
        public void Should_Success_ValidateNullVM()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var vm = new InventoryDocumentViewModel() { Items = new List<InventoryDocumentItemViewModel>() };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);
        }

        [Fact]
        public void Should_Success_ValidateNullDetailVM()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            serviceProvider.Setup(s => s.GetService(typeof(IInventoryMovementRepository)))
                .Returns(movementRepository);

            var vm = new InventoryDocumentViewModel() { Items = new List<InventoryDocumentItemViewModel>() { new InventoryDocumentItemViewModel() } };
            ValidationContext validationContext = new ValidationContext(vm, serviceProvider.Object, null);
            var response = vm.Validate(validationContext);

            Assert.True(response.Count() > 0);
        }
    }
}
