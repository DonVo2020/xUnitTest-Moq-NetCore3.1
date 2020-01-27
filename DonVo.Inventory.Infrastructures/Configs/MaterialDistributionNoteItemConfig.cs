using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class MaterialDistributionNoteItemConfig : IEntityTypeConfiguration<MaterialDistributionNoteItem>
    {
        public void Configure(EntityTypeBuilder<MaterialDistributionNoteItem> builder)
        {
            builder.Property(p => p.MaterialRequestNoteCode).HasMaxLength(100);

            builder
                .HasMany(h => h.MaterialDistributionNoteDetails)
                .WithOne(w => w.MaterialDistributionNoteItem)
                .HasForeignKey(f => f.MaterialDistributionNoteItemId)
                .IsRequired();
        }
    }
}
