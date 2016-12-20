using Microsoft.EntityFrameworkCore;

namespace Weapsy.Apps.Text.Data
{
    public interface ITextDbContextFactory
    {
        TextDbContext Create();
        TextDbContext Create(DbContextOptions<TextDbContext> options);
    }
}
