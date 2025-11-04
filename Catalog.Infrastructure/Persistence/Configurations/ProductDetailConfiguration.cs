using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(pd => pd.ProductDetailId);

            builder.Property(pd => pd.Description)
                   .HasMaxLength(1000);

            builder.Property(pd => pd.Manufacter)
                   .HasMaxLength(100);

            builder.HasOne(pd => pd.Product)
                   .WithOne(p => p.ProductDetail)
                   .HasForeignKey<ProductDetail>(pd => pd.ProductId)
                   .IsRequired(false);
        }
    }
}
