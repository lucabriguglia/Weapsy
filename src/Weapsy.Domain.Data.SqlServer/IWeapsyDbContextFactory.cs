using Microsoft.EntityFrameworkCore;

namespace Weapsy.Domain.Data.SqlServer
{
    public interface IWeapsyDbContextFactory
    {
        WeapsyDbContext Create();
        WeapsyDbContext Create(DbContextOptions<WeapsyDbContext> options);
    }
}
