using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data
{
    public interface IDbContextFactory
    {
        WeapsyDbContext Create();
        WeapsyDbContext Create(DbContextOptions options);
    }
}
