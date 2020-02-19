using System.Threading.Tasks;
using Weapsy.Core;
using Weapsy.Data;
using Weapsy.Domain.Models.Pages.Commands;

namespace Weapsy.Domain.Services.Pages
{
    public class PageService : IPageService
    {
        private readonly WeapsyDbContext _dbContext;

        public PageService(WeapsyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<CommandResponse> CreateAsync(CreatePage command)
        {
            throw new System.NotImplementedException();
        }
    }
}