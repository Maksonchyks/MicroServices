using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.DTO.ProductDetail
{
    public record ProductDetailDto(Guid ProductDetailId, string Description, string Manufacter, Guid ProductId);
}
