using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class ReturnFromBuyerItemModel : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string ColorWay { get; set; }
        [MaxLength(255)]
        public string DesignCode { get; set; }
        [MaxLength(255)]
        public string DesignNumber { get; set; }
        public double Length { get; set; }
        [MaxLength(255)]
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        [MaxLength(255)]
        public string ProductName { get; set; }
        [MaxLength(4000)]
        public string Remark { get; set; }
        public double ReturnQuantity { get; set; }
        [MaxLength(255)]
        public string Uom { get; set; }
        public int UomId { get; set; }
        public double Weight { get; set; }
        public int ReturnFromBuyerDetailId { get; set; }
        public virtual ReturnFromBuyerDetailModel ReturnFromBuyerDetail { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
