using DonVo.Inventory.ViewModels.ViewModels;
using System.Collections.Generic;

namespace DonVo.Inventory.ViewModels
{
    public class UnitReceiptNoteViewModel
    {
        public string Id { get; set; }
        public string No { get; set; }
        public CodeNameViewModel Unit { get; set; }
        public CodeNameViewModel Supplier { get; set; }
        public List<Item> Items { get; set; }
        public class Item
        {
            public CodeNameViewModel Product { get; set; }
        }

    }
}
