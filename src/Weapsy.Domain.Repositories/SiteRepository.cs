using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Weapsy.Data;
using Weapsy.Domain.Models.Sites;

namespace Weapsy.Domain.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public SiteRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<Site> GetByIdAsync(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = await context.Sites.FirstOrDefaultAsync(x => x.Id == id);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Task<bool> AnyByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Site site)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Site site)
        {
            throw new NotImplementedException();
        }
    }
}
