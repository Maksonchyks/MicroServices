using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductImage;
using FluentValidation;

namespace Catalog.Application.Validators
{
    public class ProductImageDtoValidator : AbstractValidator<ProductImageDto>
    {
        public ProductImageDtoValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Image URL is required")
                .MaximumLength(500).WithMessage("URL cannot exceed 500 characters")
                .Must(BeValidUrl).WithMessage("Invalid URL format");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
