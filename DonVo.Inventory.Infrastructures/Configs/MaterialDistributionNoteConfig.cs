using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class MaterialDistributionNoteConfig : IEntityTypeConfiguration<MaterialDistributionNote>
    {
        public void Configure(EntityTypeBuilder<MaterialDistributionNote> builder)
        {
            builder.Property(p => p.No).HasMaxLength(255);
            builder.Property(p => p.UnitId).HasMaxLength(255);
            builder.Property(p => p.UnitCode).HasMaxLength(255);
            builder.Property(p => p.UnitName).HasMaxLength(255);
            builder.Property(p => p.Type).HasMaxLength(255);

            builder
                .HasMany(h => h.MaterialDistributionNoteItems)
                .WithOne(w => w.MaterialDistributionNote)
                .HasForeignKey(f => f.MaterialDistributionNoteId)
                .IsRequired();
        }
    }
}
