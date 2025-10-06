using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public string PromoCode { get; set; } = null!;
        public decimal DiscountPercent { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
