using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class ReturnFromBuyerConfig : IEntityTypeConfiguration<ReturnFromBuyerModel>
    {
        public void Configure(EntityTypeBuilder<ReturnFromBuyerModel> builder)
        {
            builder
                .HasMany(h => h.Details)
                .WithOne(w => w.ReturnFromBuyer)
                .HasForeignKey(f => f.ReturnFromBuyerId)
                .IsRequired();
        }
    }
}
