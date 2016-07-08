using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Events;
using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Weapsy.Services.Identity
{
    public class RoleCreatedHandler : IEventHandler<RoleCreated>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public RoleCreatedHandler(RoleManager<IdentityRole> roleManager, 
            ILoggerFactory loggerFactory)
        {
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<RoleCreatedHandler>();
        }

        public async Task Handle(RoleCreated @event)
        {
            var role = new IdentityRole
            {
                Id = @event.AggregateRootId.ToString(),
                Name = @event.Name
            };

            var identityResult = await _roleManager.CreateAsync(role);

            if (!identityResult.Succeeded)
            {
                var errorMessage = GetErrorMessage(identityResult);
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }                
        }

        private string GetErrorMessage(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }
    }
}
