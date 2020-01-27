using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class InventoryDocumentConfig : IEntityTypeConfiguration<InventoryDocument>
    {
        public void Configure(EntityTypeBuilder<InventoryDocument> builder)
        {
            builder.Property(p => p.No).HasMaxLength(255);
            builder.Property(p => p.ReferenceNo).HasMaxLength(255);
            builder.Property(p => p.ReferenceType).HasMaxLength(255);
            builder.Property(p => p.StorageId).HasMaxLength(255);
            builder.Property(p => p.StorageCode).HasMaxLength(255);
            builder.Property(p => p.StorageName).HasMaxLength(255);

            builder
                .HasMany(h => h.Items)
                .WithOne(w => w.InventoryDocument)
                .HasForeignKey(f => f.InventoryDocumentId)
                .IsRequired();
        }
    }
}

