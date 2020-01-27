using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class RegradingResultDocsConfig : IEntityTypeConfiguration<RegradingResultDocs>
    {
        public void Configure(EntityTypeBuilder<RegradingResultDocs> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(255);
            builder.Property(p => p.NoBonId).HasMaxLength(128);
            builder.Property(p => p.NoBon).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(255);
            builder
                .HasMany(h => h.Details)
                .WithOne(w => w.ReturProInvDocs)
                .HasForeignKey(f => f.ReturProInvDocsId)
                .IsRequired();
        }
    }
}
