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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Content як owned type
            builder.OwnsOne(r => r.Content, contentBuilder =>
            {
                contentBuilder.Property(cnt => cnt.Text)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .HasColumnName("ContentText");

                contentBuilder.Property(cnt => cnt.WordCount)
                    .IsRequired()
                    .HasColumnName("ContentWordCount");
            });

            // Rating як owned type
            builder.OwnsOne(r => r.Rating, ratingBuilder =>
            {
                ratingBuilder.Property(rt => rt.Score)
                    .IsRequired()
                    .HasColumnName("RatingScore");

                ratingBuilder.Property(rt => rt.MaxScore)
                    .IsRequired()
                    .HasColumnName("RatingMaxScore");
            });

            // UserInfo як owned type
            builder.OwnsOne(r => r.Author, navBuilder =>
            {
                navBuilder.Property(a => a.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("AuthorUserId");

                navBuilder.Property(a => a.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("AuthorUserName");

                navBuilder.OwnsOne(a => a.Email, emailBuilder =>
                {
                    emailBuilder.Property(e => e.Value)
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnName("AuthorEmail");
                });
            });

            builder.Property(r => r.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.HelpfulCount)
                .HasDefaultValue(0);

            builder.Property(r => r.UnhelpfulCount)
                .HasDefaultValue(0);

            // Attachments як JSON
            builder.Property(r => r.Attachments)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
                )
                .HasColumnType("nvarchar(2000)"); // Для SQL Server

            builder.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.IsVerifiedPurchase)
                .HasDefaultValue(false);

            builder.Property(r => r.IsEdited)
                .HasDefaultValue(false);

            builder.Property(r => r.EditedAt)
                .IsRequired(false);

            // Індекси
            builder.HasIndex(r => r.ProductId);
            builder.HasIndex(r => r.Status);
            builder.HasIndex(r => new { r.ProductId, r.Status });
            builder.HasIndex(r => r.CreatedAt);
        }
    }

}
