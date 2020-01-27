using DonVo.Inventory.ViewModels.ViewModels;

namespace DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel
{
    public class MaterialDistributionNoteDetailViewModel : BasicViewModel
    {
        public int MaterialDistributionNoteItemId { get; set; }
        public int MaterialsRequestNoteItemId { get; set; }
        public string Grade { get; set; }
        public double? Quantity { get; set; }
        public double? MaterialRequestNoteItemLength { get; set; }
        public double? DistributedLength { get; set; }
        public double? ReceivedLength { get; set; }
        public bool IsDisposition { get; set; }
        public bool IsCompleted { get; set; }
        public CodeNameViewModel Supplier { get; set; }
        public ProductionOrderViewModel ProductionOrder { get; set; }
        public CodeNameViewModel Product { get; set; }
    }
}
