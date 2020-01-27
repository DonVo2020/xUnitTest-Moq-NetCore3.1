using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class MaterialsRequestNoteConfig : IEntityTypeConfiguration<MaterialsRequestNote>
    {
        public void Configure(EntityTypeBuilder<MaterialsRequestNote> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(100);
            builder.Property(p => p.UnitId).HasMaxLength(100);
            builder.Property(p => p.UnitCode).HasMaxLength(100);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.Remark).HasMaxLength(1000);
            builder.Property(p => p.RequestType).HasMaxLength(100);
            builder
                .HasMany(h => h.MaterialsRequestNoteItems)
                .WithOne(w => w.MaterialsRequestNote)
                .HasForeignKey(f => f.MaterialsRequestNoteId)
                .IsRequired();
        }
    }
}
