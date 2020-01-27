using DonVo.Inventory.ViewModels.ViewModels;

namespace DonVo.Inventory.ViewModels.FPReturnInvToPurchasingViewModel
{
    public class ReturnInvToPurchasingDetailViewModel : BasicViewModel
    {
        public int ReturnInvToPurchasingId { get; set; }
        public int RegradingResultDocsId { get; set; }
        public string RegradingResultDocsCode { get; set; }       
        public double Quantity { get; set; }
        public double NecessaryLength { get; set; }
        public double Length { get; set; }
        public string Description { get; set; }
        public CodeNameViewModel Product { get; set; }
        public ReturnInvToPurchasingDetailViewModel() { }

    }
}
