using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.xUnitTests.Repository;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DonVo.Year2020.Service.Inventory.Test.Facades.Inventory
{
    public class InventoryMovementReportBasicTest
    {
        readonly Mock<IServiceProvider> serviceProvider;
        readonly InventoryDbContext inventoryDbContext;
        readonly InventoryMovementRepository repository;
        readonly InventoryMovementSampleData movementSampleData;
        readonly InventorySummaryRepository summaryRepository;
        readonly string getCurrentMethod;

        public InventoryMovementReportBasicTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
            repository = new InventoryMovementRepository(serviceProvider.Object, inventoryDbContext);
            movementSampleData = TestSetup.MovementSampleData(repository);
            summaryRepository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = movementSampleData.GetNewData();
            var Response = await repository.Create(data);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = movementSampleData.GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.Create(null));
        }

        [Fact]
        public void Should_Success_GenerateExcel()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = movementSampleData.GetTestData();
            var Response = repository.GenerateExcel(null, null, null, null, null, 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_Success_GetReport()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = movementSampleData.GetTestData();
            var Response = repository.GetReport(null, null, null, null, null, 1, 25, "{}", 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = movementSampleData.GetNewDataViewModel();
            var model = repository.MapToModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = await movementSampleData.GetTestData();
            var model = repository.MapToViewModel(data);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = await movementSampleData.GetTestData();
            var model = repository.Read(1, 25, "{}", null, "{}");

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = await movementSampleData.GetTestData();
            var model = repository.ReadModelById(data.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_RefreshMovememnt()
        {
            serviceProvider.Setup(s => s.GetService(typeof(IInventorySummaryRepository)))
                .Returns(summaryRepository);
            var data = await movementSampleData.GetTestData();
            var result = await repository.RefreshInventoryMovement();
            Assert.NotEqual(0, result);
        }
    }
}