using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class ReturnInvToPurchasingDetailConfig : IEntityTypeConfiguration<ReturnInvToPurchasingDetail>
    {
        public void Configure(EntityTypeBuilder<ReturnInvToPurchasingDetail> builder)
        {
            builder.Property(p => p.RegradingResultDocsCode).HasMaxLength(255);
            builder.Property(p => p.ProductId).HasMaxLength(255);
            builder.Property(p => p.ProductCode).HasMaxLength(255);
            builder.Property(p => p.ProductName).HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(2000);
        }
    }
}
