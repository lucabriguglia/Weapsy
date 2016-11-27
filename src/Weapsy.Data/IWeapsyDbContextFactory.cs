using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data
{
    public interface IWeapsyDbContextFactory
    {
        WeapsyDbContext Create();
        WeapsyDbContext Create(DbContextOptions<WeapsyDbContext> options);
    }
}
