using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class RegradingResultDocsDetailsConfig : IEntityTypeConfiguration<RegradingResultDocsDetails>
    {
        public void Configure(EntityTypeBuilder<RegradingResultDocsDetails> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(255);
        }
    }
}
