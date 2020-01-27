using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class StockTransferNoteConfig : IEntityTypeConfiguration<StockTransferNote>
    {
        public void Configure(EntityTypeBuilder<StockTransferNote> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(255);
            builder.Property(p => p.ReferenceNo).HasMaxLength(255);
            builder.Property(p => p.ReferenceType).HasMaxLength(255);
            builder.Property(p => p.SourceStorageId).HasMaxLength(255);
            builder.Property(p => p.SourceStorageCode).HasMaxLength(255);
            builder.Property(p => p.SourceStorageName).HasMaxLength(255);
            builder.Property(p => p.TargetStorageId).HasMaxLength(255);
            builder.Property(p => p.TargetStorageCode).HasMaxLength(255);
            builder.Property(p => p.TargetStorageName).HasMaxLength(255);

            builder
                .HasMany(h => h.StockTransferNoteItems)
                .WithOne(w => w.StockTransferNote)
                .HasForeignKey(f => f.StockTransferNoteId)
                .IsRequired();
        }
    }
}
