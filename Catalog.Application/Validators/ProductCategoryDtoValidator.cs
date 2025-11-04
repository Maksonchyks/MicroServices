using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductCategory;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class ProductCategoryDtoValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required");
        }
    }
}
