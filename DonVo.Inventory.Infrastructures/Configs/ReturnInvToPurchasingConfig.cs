using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class ReturnInvToPurchasingConfig : IEntityTypeConfiguration<ReturnInvToPurchasing>
    {
        public void Configure(EntityTypeBuilder<ReturnInvToPurchasing> builder)
        {
            builder.Property(p => p.No).HasMaxLength(255);
            builder.Property(p => p.UnitName).HasMaxLength(255);
            builder.Property(p => p.SupplierId).HasMaxLength(255);
            builder.Property(p => p.SupplierCode).HasMaxLength(255);
            builder.Property(p => p.SupplierName).HasMaxLength(255);

            builder
                .HasMany(h => h.ReturnInvToPurchasingDetails)
                .WithOne(w => w.ReturnInvToPurchasing)
                .HasForeignKey(f => f.ReturnInvToPurchasingId)
                .IsRequired();
        }
    }
}
