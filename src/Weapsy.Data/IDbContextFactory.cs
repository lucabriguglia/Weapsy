using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data
{
    public interface IDbContextFactory
    {
        BaseDbContext Create();
        BaseDbContext Create(DbContextOptions options);
    }
}
