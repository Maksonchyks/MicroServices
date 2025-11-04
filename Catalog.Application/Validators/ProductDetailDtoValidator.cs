using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductDetail;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class ProductDetailDtoValidator : AbstractValidator<ProductDetailDto>
    {
        public ProductDetailDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Manufacter)
                .NotEmpty().WithMessage("Manufacturer is required")
                .MaximumLength(100).WithMessage("Manufacturer cannot exceed 100 characters");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");
        }
    }
}
