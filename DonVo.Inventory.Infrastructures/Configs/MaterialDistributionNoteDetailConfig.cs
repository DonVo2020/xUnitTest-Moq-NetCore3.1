using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class MaterialDistributionNoteDetailConfig : IEntityTypeConfiguration<MaterialDistributionNoteDetail>
    {
        public void Configure(EntityTypeBuilder<MaterialDistributionNoteDetail> builder)
        {
            builder.Property(p => p.ProductionOrderNo).HasMaxLength(255);
            builder.Property(p => p.ProductId).HasMaxLength(255);
            builder.Property(p => p.ProductCode).HasMaxLength(255);
            builder.Property(p => p.ProductName).HasMaxLength(255);
            builder.Property(p => p.Grade).HasMaxLength(255);
            builder.Property(p => p.SupplierId).HasMaxLength(255);
            builder.Property(p => p.SupplierCode).HasMaxLength(255);
            builder.Property(p => p.SupplierName).HasMaxLength(255);
        }
    }
}
