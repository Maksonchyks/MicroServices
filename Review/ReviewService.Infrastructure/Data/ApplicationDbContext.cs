using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Discussion> Discussions => Set<Discussion>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<RatingEntity> RatingEntities => Set<RatingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Застосування всіх конфігурацій
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global query filters для soft delete
            modelBuilder.Entity<Discussion>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<RatingEntity>().HasQueryFilter(r => !r.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Автоматичне оновлення UpdatedAt та Version
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                entry.Entity.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Version++;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
