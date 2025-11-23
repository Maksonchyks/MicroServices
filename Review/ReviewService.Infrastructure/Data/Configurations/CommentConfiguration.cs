using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasMaxLength(50)
                .IsRequired();

            // Content як owned type
            builder.OwnsOne(c => c.Content, contentBuilder =>
            {
                contentBuilder.Property(cnt => cnt.Text)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .HasColumnName("ContentText");

                contentBuilder.Property(cnt => cnt.WordCount)
                    .IsRequired()
                    .HasColumnName("ContentWordCount");
            });

            // UserInfo як owned type
            builder.OwnsOne(c => c.Author, navBuilder =>
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

            builder.Property(c => c.DiscussionId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ParentCommentId)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(c => c.LikeCount)
                .HasDefaultValue(0);

            builder.Property(c => c.DislikeCount)
                .HasDefaultValue(0);

            builder.Property(c => c.ReplyCount)
                .HasDefaultValue(0);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.IsEdited)
                .HasDefaultValue(false);

            builder.Property(c => c.EditedAt)
                .IsRequired(false);

            // Зв'язки
            builder.HasOne<Discussion>()
                .WithMany()
                .HasForeignKey(c => c.DiscussionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Індекси
            builder.HasIndex(c => c.DiscussionId);
            builder.HasIndex(c => c.ParentCommentId);
            builder.HasIndex(c => c.CreatedAt);
            builder.HasIndex(c => new { c.DiscussionId, c.ParentCommentId });
        }
    }

}
