using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.ViewModels.FpReturnFromBuyers
{
    public class ReturnFromBuyerItemViewModel : BasicViewModel, IValidatableObject
    {
        public string ColorWay { get; set; }
        public string DesignCode { get; set; }
        public string DesignNumber { get; set; }
        public double Length { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Remark { get; set; }
        public double ReturnQuantity { get; set; }
        public string Uom { get; set; }
        public int UomId { get; set; }
        public double Weight { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
