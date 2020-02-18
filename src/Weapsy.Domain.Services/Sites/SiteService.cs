using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Weapsy.Data;
using Weapsy.Data.Entities;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Services.Sites
{
    public class SiteService : ISiteService
    {
        private readonly WeapsyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSite> _createSiteValidator;
        private readonly IValidator<UpdateSite> _updateSiteValidator;

        public SiteService(WeapsyDbContext dbContext, 
            IMapper mapper, 
            IValidator<CreateSite> createSiteValidator, 
            IValidator<UpdateSite> updateSiteValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createSiteValidator = createSiteValidator;
            _updateSiteValidator = updateSiteValidator;
        }

        public async Task CreateAsync(CreateSite command)
        {
            await _createSiteValidator.ValidateAndThrowAsync(command);

            var site = new Site(command);
            var siteEntity = _mapper.Map<SiteEntity>(site);

            _dbContext.Sites.Add(siteEntity);

            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(UpdateSite command)
        {
            throw new NotImplementedException();
        }
    }
}
