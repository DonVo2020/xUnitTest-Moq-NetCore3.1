using DonVo.Inventory.Domains;
using DonVo.Inventory.Domains.Repositories;
using DonVo.Inventory.xUnitTests.Repository.Helpers;
using DonVo.Inventory.xUnitTests.Repository.SampleData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Diagnostics;

namespace DonVo.Inventory.xUnitTests.Repository
{
    public class TestSetup
    {
        private const string ENTITY = "InventoryDocument";

        public static Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpServiceRepository)))
                .Returns(new HttpTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityInterface)))
                .Returns(new IdentityInterface() { Token = "Token", Username = "Test" });

            return serviceProvider;
        }

        public static Mock<IServiceProvider> GetFailServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpServiceRepository)))
                .Returns(new HttpFailTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityInterface)))
                .Returns(new IdentityInterface() { Token = "Token", Username = "Test" });

            return serviceProvider;
        }

        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        public static InventoryDbContext DbContext(string testName)
        {
            DbContextOptionsBuilder<InventoryDbContext> optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            InventoryDbContext dbContext = new InventoryDbContext(optionsBuilder.Options);

            return dbContext;
        }

        public static InventoryDocumentSampleData DocumentSampleData(InventoryDocumentRepository service)
        {
            return new InventoryDocumentSampleData(service);
        }

        public static InventoryMovementSampleData MovementSampleData(InventoryMovementRepository service)
        {
            return new InventoryMovementSampleData(service);
        }

        public static InventorySummarySampleData SummarySampleData(InventorySummaryRepository service)
        {
            return new InventorySummarySampleData(service);
        }
    }
}
