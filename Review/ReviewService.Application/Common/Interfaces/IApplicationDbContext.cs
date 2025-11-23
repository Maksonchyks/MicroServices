
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;

namespace ReviewService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Discussion> Discussions { get; }
        DbSet<Comment> Comments { get; }
        DbSet<RatingEntity> RatingEntities { get; }
        DbSet<Review> Reviews { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
