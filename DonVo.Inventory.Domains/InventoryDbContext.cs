using DonVo.Inventory.Infrastructures.Configs;
using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DonVo.Inventory.Domains
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext([NotNull] DbContextOptions options) : base(options)
        {

        }

        public DbSet<MaterialsRequestNote> MaterialsRequestNotes { get; set; }
        public DbSet<MaterialsRequestNoteItem> MaterialsRequestNoteItems { get; set; }
        public DbSet<RegradingResultDocsDetails> RegradingResultDocsDetails { get; set; }
        public DbSet<RegradingResultDocs> RegradingResultDocs { get; set; }
        public DbSet<MaterialDistributionNote> MaterialDistributionNotes { get; set; }
        public DbSet<MaterialDistributionNoteItem> MaterialDistributionNoteItems { get; set; }
        public DbSet<MaterialDistributionNoteDetail> MaterialDistributionNoteDetails { get; set; }
        public DbSet<StockTransferNote> StockTransferNotes { get; set; }
        public DbSet<StockTransferNoteItem> StockTransferNoteItems { get; set; }
        public DbSet<ReturnInvToPurchasing> ReturnInvToPurchasings { get; set; }
        public DbSet<ReturnInvToPurchasingDetail> ReturnInvToPurchasingDetails { get; set; }
        public DbSet<InventoryDocument> InventoryDocuments { get; set; }
        public DbSet<InventoryDocumentItem> InventoryDocumentItems { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<InventorySummary> InventorySummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MaterialsRequestNoteConfig());
            modelBuilder.ApplyConfiguration(new MaterialsRequestNoteItemConfig());
            modelBuilder.ApplyConfiguration(new RegradingResultDocsDetailsConfig());
            modelBuilder.ApplyConfiguration(new RegradingResultDocsConfig());
            modelBuilder.ApplyConfiguration(new MaterialDistributionNoteConfig());
            modelBuilder.ApplyConfiguration(new MaterialDistributionNoteItemConfig());
            modelBuilder.ApplyConfiguration(new MaterialDistributionNoteDetailConfig());
            modelBuilder.ApplyConfiguration(new StockTransferNoteConfig());
            modelBuilder.ApplyConfiguration(new StockTransferNoteItemConfig());
            modelBuilder.ApplyConfiguration(new ReturnInvToPurchasingConfig());
            modelBuilder.ApplyConfiguration(new ReturnInvToPurchasingDetailConfig());
            modelBuilder.ApplyConfiguration(new InventoryDocumentConfig());
            modelBuilder.ApplyConfiguration(new InventoryDocumentItemConfig());
            modelBuilder.ApplyConfiguration(new InventoryMovementConfig());
            modelBuilder.ApplyConfiguration(new InventorySummaryConfig());
        }
    }
}
