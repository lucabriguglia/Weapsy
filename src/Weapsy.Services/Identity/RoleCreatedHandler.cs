using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Events;
using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles;

namespace Weapsy.Services.Identity
{
    public class RoleCreatedHandler : IEventHandler<RoleCreated>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ICommandSender _commandSender;

        public RoleCreatedHandler(RoleManager<IdentityRole> roleManager, 
            ILoggerFactory loggerFactory,
            ICommandSender commandSender)
        {
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<RoleCreatedHandler>();
            _commandSender = commandSender;
        }

        public async Task Handle(RoleCreated @event)
        {
            var role = new IdentityRole
            {
                Id = @event.AggregateRootId.ToString(),
                Name = @event.Name
            };

            IdentityResult identityResult;

            try
            {
                identityResult = await _roleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                HandleError(@event, ex.Message);
                throw;
            }
            
            if (!identityResult.Succeeded)
            {
                var errorMessage = GetErrorMessage(identityResult);
                HandleError(@event, errorMessage);
            }                
        }

        private void HandleError(RoleCreated @event, string errorMessage)
        {
            _logger.LogError(errorMessage);
            _commandSender.Send<DestroyRole, Role>(new DestroyRole
            {
                Id = @event.AggregateRootId,
                Name = @event.Name
            });
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
