using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class MaterialsRequestNoteItemConfig : IEntityTypeConfiguration<MaterialsRequestNoteItem>
    {
        public void Configure(EntityTypeBuilder<MaterialsRequestNoteItem> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(100);
            builder.Property(p => p.ProductionOrderId).HasMaxLength(100);
            builder.Property(p => p.ProductionOrderNo).HasMaxLength(100);
            builder.Property(p => p.OrderTypeId).HasMaxLength(100);
            builder.Property(p => p.OrderTypeCode).HasMaxLength(100);
            builder.Property(p => p.OrderTypeName).HasMaxLength(100);
            builder.Property(p => p.ProductId).HasMaxLength(100);
            builder.Property(p => p.ProductCode).HasMaxLength(100);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.Grade).HasMaxLength(500);
        }
    }
}
