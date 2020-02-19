using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weapsy.Core.Domain;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Services.Sites;

namespace Weapsy.Web.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SiteController : ControllerBase
    {
        private readonly ILogger<SiteController> _logger;
        private readonly IProcessor _processor;
        private readonly ISiteService _siteService;

        public SiteController(ILogger<SiteController> logger, IProcessor processor, ISiteService siteService)
        {
            _logger = logger;
            _processor = processor;
            _siteService = siteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSite(string name)
        {
            var command = new CreateSite
            {
                Name = name
            };

            await _processor.ProcessAsync(() => _siteService.CreateAsync(command));

            return NoContent();
        }
    }
}
