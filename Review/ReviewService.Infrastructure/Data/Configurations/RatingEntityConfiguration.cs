using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data.Configurations
{
    public class RatingEntityConfiguration : IEntityTypeConfiguration<RatingEntity>
    {
        public void Configure(EntityTypeBuilder<RatingEntity> builder)
        {
            builder.ToTable("RatingEntities");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(50);

            // Rating як owned type
            builder.OwnsOne(r => r.RatingValue, ratingBuilder =>
            {
                ratingBuilder.Property(rt => rt.Score)
                    .IsRequired()
                    .HasColumnName("RatingScore");

                ratingBuilder.Property(rt => rt.MaxScore)
                    .IsRequired()
                    .HasColumnName("RatingMaxScore");
            });

            builder.Property(r => r.ReviewId)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(r => r.RatedAt)
                .IsRequired();

            // Унікальний індекс - один користувач один рейтинг на продукт
            builder.HasIndex(r => new { r.ProductId, r.UserId })
                .IsUnique();

            // Індекси
            builder.HasIndex(r => r.ProductId);
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.RatedAt);
        }
    }

}
