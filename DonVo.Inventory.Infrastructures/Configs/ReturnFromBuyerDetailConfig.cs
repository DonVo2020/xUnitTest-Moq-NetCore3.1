using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class ReturnFromBuyerDetailConfig : IEntityTypeConfiguration<ReturnFromBuyerDetailModel>
    {
        public void Configure(EntityTypeBuilder<ReturnFromBuyerDetailModel> builder)
        {
            builder
                .HasMany(h => h.Items)
                .WithOne(w => w.ReturnFromBuyerDetail)
                .HasForeignKey(f => f.ReturnFromBuyerDetailId)
                .IsRequired();
        }
    }
}
