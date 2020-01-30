using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Weapsy.Domain.Handlers.Sites;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Repositories;
using Weapsy.Domain.Validators.Sites;
using Weapsy.Reporting.Handlers;
using Weapsy.Reporting.Models.Sites.Queries;

namespace Weapsy.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IWeapsyServiceBuilder AddWeapsy(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var kledexServiceBuilder = services.AddKledex(typeof(CreateSite), 
                typeof(CreateSiteHandler),
                typeof(CreateSiteValidator),
                typeof(SiteRepository),
                typeof(GetSiteInfo),
                typeof(GetSiteInfoHandler));

            return new WeapsyServiceBuilder(services);
        }
    }
}
