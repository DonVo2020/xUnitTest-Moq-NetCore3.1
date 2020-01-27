namespace DonVo.Inventory.ViewModels.ViewModels.ReturnFromBuyers
{
    public class ProductionOrderIntegrationViewModel
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public double? OrderQuantity { get; set; }
        public bool IsCompleted { get; set; }
        public double? DistributedQuantity { get; set; }
        public OrderTypeIntegrationViewModel OrderType { get; set; }
    }
}
