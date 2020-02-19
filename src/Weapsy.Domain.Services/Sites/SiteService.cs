using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Weapsy.Core.Domain;
using Weapsy.Data;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Models.Sites.Events;

namespace Weapsy.Domain.Services.Sites
{
    public class SiteService : ISiteService
    {
        private readonly WeapsyDbContext _dbContext;
        private readonly IValidator<CreateSite> _createSiteValidator;
        private readonly IValidator<UpdateSite> _updateSiteValidator;

        public SiteService(WeapsyDbContext dbContext,
            IValidator<CreateSite> createSiteValidator, 
            IValidator<UpdateSite> updateSiteValidator)
        {
            _dbContext = dbContext;
            _createSiteValidator = createSiteValidator;
            _updateSiteValidator = updateSiteValidator;
        }

        public async Task<CommandResponse> CreateAsync(CreateSite command)
        {
            await _createSiteValidator.ValidateAndThrowAsync(command);

            var site = new Site(command);

            _dbContext.Sites.Add(site);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse(new SiteCreated
            {
                SiteId = site.Id,
                Name = site.Name
            });
        }

        public async Task<CommandResponse> UpdateAsync(UpdateSite command)
        {
            var site = await _dbContext.Sites.FirstAsync(x => x.Id == command.SiteId);

            if (site == null)
            {
                throw new ApplicationException($"Site with Id {command.SiteId} not found.");
            }

            await _updateSiteValidator.ValidateAndThrowAsync(command);

            site.Update(command);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse(new SiteUpdated
            {
                SiteId = site.Id,
                Name = site.Name
            });
        }
    }
}
