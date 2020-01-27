using DonVo.Inventory.ViewModels.ViewModels;

namespace DonVo.Inventory.ViewModels
{
    public class ProductionOrderViewModel
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public double? OrderQuantity { get; set; }
        public bool IsCompleted { get; set; }
        public double? DistributedQuantity { get; set; }
        public CodeNameViewModel OrderType { get; set; }
    }
}
