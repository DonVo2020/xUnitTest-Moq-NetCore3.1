using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class InventoryDocumentItemConfig : IEntityTypeConfiguration<InventoryDocumentItem>
    {
        public void Configure(EntityTypeBuilder<InventoryDocumentItem> builder)
        {
            builder.Property(p => p.ProductId).HasMaxLength(255);
            builder.Property(p => p.ProductCode).HasMaxLength(1000);
            builder.Property(p => p.ProductName).HasMaxLength(4000);
            builder.Property(p => p.UomUnit).HasMaxLength(255);
            builder.Property(p => p.UomId).HasMaxLength(255);
        }
    }
}
