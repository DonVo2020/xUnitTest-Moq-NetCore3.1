using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class InventorySummaryConfig : IEntityTypeConfiguration<InventorySummary>
    {
        public void Configure(EntityTypeBuilder<InventorySummary> builder)
        {
            builder.Property(p => p.No).HasMaxLength(255);
            builder.Property(p => p.StorageId).HasMaxLength(255);
            builder.Property(p => p.StorageCode).HasMaxLength(255);
            builder.Property(p => p.StorageName).HasMaxLength(255);
            builder.Property(p => p.ProductId).HasMaxLength(255);
            builder.Property(p => p.ProductCode).HasMaxLength(1000);
            builder.Property(p => p.ProductName).HasMaxLength(4000);
            builder.Property(p => p.UomUnit).HasMaxLength(255);
            builder.Property(p => p.UomId).HasMaxLength(255);
        }
    }
}
