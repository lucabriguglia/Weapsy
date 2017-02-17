using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Data.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<WeapsyDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
