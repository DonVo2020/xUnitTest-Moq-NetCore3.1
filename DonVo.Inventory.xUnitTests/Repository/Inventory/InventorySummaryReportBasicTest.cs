using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.xUnitTests.Repository;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DonVo.Year2020.Service.Inventory.Test.Facades.Inventory
{
    public class InventorySummaryReportBasicTest
    {
        readonly Mock<IServiceProvider> serviceProvider;       
        readonly InventoryDbContext inventoryDbContext;
        readonly InventorySummaryRepository repository;
        private readonly InventorySummarySampleData summarySampleData;
        readonly string getCurrentMethod;

        public InventorySummaryReportBasicTest()
        {
            serviceProvider = TestSetup.GetServiceProvider();
            getCurrentMethod = TestSetup.GetCurrentMethod();
            inventoryDbContext = TestSetup.DbContext(getCurrentMethod);
            repository = new InventorySummaryRepository(serviceProvider.Object, inventoryDbContext);
            summarySampleData = TestSetup.SummarySampleData(repository);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            var data = summarySampleData.GetNewData();
            var Response = await repository.Create(data);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            var data = summarySampleData.GetNewData();
            await Assert.ThrowsAnyAsync<Exception>(() => repository.Create(null));
        }

        [Fact]
        public void Should_Success_GenerateExcel()
        {
            var data = summarySampleData.GetTestData();
            var Response = repository.GenerateExcel(null, null, 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_Success_GetReport()
        {
            var data = summarySampleData.GetTestData();
            var Response = repository.GetReport(null, null, 1, 25, "{}", 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_Success_MapToModel()
        {
            var data = summarySampleData.GetNewDataViewModel();
            var model = repository.MapToModel(data);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            var data = await summarySampleData.GetTestData();
            var model = repository.MapToViewModel(data);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_Read()
        {
            var data = await summarySampleData.GetTestData();
            var model = repository.Read(1, 25, "{}", null, "{}");
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_ReadId()
        {
            var data = await summarySampleData.GetTestData();
            var data2 = summarySampleData.GetNewData();
            var Response = await repository.Create(data2);
            var models = repository.Read(1, 25, "{}", null, "{}");
            var single = models.Data.FirstOrDefault();
            var model = repository.ReadModelById(single.Id);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_GetStorageMTR()
        {
            var data = await summarySampleData.GetTestData();
            var models = repository.Read(1, 25, "{}", null, "{}");
            var single = models.Data.FirstOrDefault();
            var model = repository.GetByStorageAndMTR(single.StorageName);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_GetInventorySummaries()
        {
            var data = await summarySampleData.GetTestData();
            var models = repository.Read(1, 25, "{}", null, "{}");
            var single = models.Data.FirstOrDefault();
            Dictionary<string, object> prdIds = new Dictionary<string, object>
            {
                { "Id", new List<int>(){ single.ProductId } }
            };
            var model = repository.GetInventorySummaries(JsonConvert.SerializeObject(prdIds));

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Should_Success_GetSummaryByParam()
        {
            var data = await summarySampleData.GetTestData();
            var models = repository.Read(1, 25, "{}", null, "{}");
            var single = models.Data.FirstOrDefault();
            var model = repository.GetSummaryByParams(single.StorageId, single.ProductId, single.UomId);

            Assert.NotNull(model);
        }
    }
}