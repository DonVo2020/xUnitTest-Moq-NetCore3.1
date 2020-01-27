using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class ReturnFromBuyerDetailModel : StandardEntity, IValidatableObject
    {
        public ICollection<ReturnFromBuyerItemModel> Items { get; set; }
        //public ProductionOrderViewModel ProductionOrder { get; set; }
        public int ProductionOrderId { get; set; }
        [MaxLength(255)]
        public string ProductionOrderNo { get; set; }
        public int ReturnFromBuyerId { get; set; }
        public virtual ReturnFromBuyerModel ReturnFromBuyer { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
