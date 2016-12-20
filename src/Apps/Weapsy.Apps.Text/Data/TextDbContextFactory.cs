using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Apps.Text.Data
{
    public class TextDbContextFactory : ITextDbContextFactory
    {
        private ConnectionStrings ConnectionStrings { get; }

        public TextDbContextFactory(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public TextDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TextDbContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.DefaultConnection);

            return new TextDbContext(optionsBuilder.Options);
        }

        public TextDbContext Create(DbContextOptions<TextDbContext> options)
        {
            return new TextDbContext(options);
        }
    }
}
