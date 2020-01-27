using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DonVo.Inventory.Models
{
    public class ReturnFromBuyerModel : StandardEntity, IValidatableObject
    {
        //public BuyerViewModel Buyer { get; set; }
        public int BuyerId { get; set; }
        [MaxLength(255)]
        public string BuyerCode { get; set; }
        [MaxLength(255)]
        public string BuyerName { get; set; }
        [MaxLength(255)]
        public string Code { get; set; }
        [MaxLength(255)]
        public string CodeProduct { get; set; }
        [MaxLength(255)]
        public string CoverLetter { get; set; }
        public DateTimeOffset Date { get; set; }
        [MaxLength(255)]
        public string Destination { get; set; }
        public ICollection<ReturnFromBuyerDetailModel> Details { get; set; }
        [MaxLength(255)]
        public string SpkNo { get; set; }
        //public StorageViewModel Storage { get; set; }
        public int StorageId { get; set; }
        [MaxLength(255)]
        public string StorageCode { get; set; }
        [MaxLength(255)]
        public string StorageName { get; set; }
        public bool IsVoid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
