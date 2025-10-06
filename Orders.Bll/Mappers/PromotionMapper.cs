using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Domain.Enteties;

namespace Orders.Bll.Mappers
{
    public static class PromotionMapper
    {
        public static PromotionDto ToDto(Promotion promo)
        {
            if (promo == null) throw new ArgumentNullException(nameof(promo));

            return new PromotionDto
            {
                PromotionId = promo.PromotionId,
                PromoCode = promo.PromoCode,
                DiscountPercent = promo.DiscountPercent,
                ExpiryDate = promo.ExpiryDate
            };
        }

        public static Promotion ToEntity(PromotionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Promotion
            {
                PromotionId = dto.PromotionId,
                PromoCode = dto.PromoCode,
                DiscountPercent = dto.DiscountPercent,
                ExpiryDate = dto.ExpiryDate
            };
        }
    }
}
