using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Apps.Text.Data;
using Weapsy.Mvc.Apps;

namespace Weapsy.Apps.Text
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void Configure(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<TextDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
