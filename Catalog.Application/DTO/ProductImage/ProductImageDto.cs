using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.DTO.ProductImage
{
    public record ProductImageDto(Guid ProductImageId, string Url, Guid ProductId);

}
