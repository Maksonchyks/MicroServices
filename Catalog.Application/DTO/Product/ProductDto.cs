using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.DTO.Product
{
    public record ProductDto(Guid Id, string Name, decimal Price);

}
