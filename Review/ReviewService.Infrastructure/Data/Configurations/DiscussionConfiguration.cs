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
    public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder.ToTable("Discussions");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(d => d.Category)
                .IsRequired()
                .HasMaxLength(50);

            // Tags як JSON
            builder.Property(d => d.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
                )
                .HasColumnType("nvarchar(1000)"); // Для SQL Server
                                                  // .HasColumnType("jsonb"); // Для PostgreSQL

            // UserInfo як owned type
            builder.OwnsOne(d => d.Author, navBuilder =>
            {
                navBuilder.Property(a => a.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("AuthorUserId");

                navBuilder.Property(a => a.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("AuthorUserName");

                // Email як вкладений owned type
                navBuilder.OwnsOne(a => a.Email, emailBuilder =>
                {
                    emailBuilder.Property(e => e.Value)
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnName("AuthorEmail");
                });
            });

            builder.Property(d => d.ViewCount)
                .HasDefaultValue(0);

            builder.Property(d => d.CommentCount)
                .HasDefaultValue(0);

            builder.Property(d => d.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(d => d.PinnedAt)
                .IsRequired(false);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.UpdatedAt)
                .IsRequired();

            builder.Property(d => d.Version)
                .IsRequired()
                .HasDefaultValue(1)
                .IsConcurrencyToken(); // Для optimistic concurrency

            builder.Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            // Індекси
            builder.HasIndex(d => d.Category);
            builder.HasIndex(d => d.Status);
            builder.HasIndex(d => d.CreatedAt);
            builder.HasIndex(d => new { d.IsDeleted, d.Status });
        }
    }

}
