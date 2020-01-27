using DonVo.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonVo.Inventory.Infrastructures.Configs
{
    public class ReturnFromBuyerItemConfig : IEntityTypeConfiguration<ReturnFromBuyerItemModel>
    {
        public void Configure(EntityTypeBuilder<ReturnFromBuyerItemModel> builder)
        {
        }
    }
}
