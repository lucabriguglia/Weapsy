using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Events;
using System;
using System.Text;

namespace Weapsy.Domain.Model.Users.Handlers
{
    public class RoleCreatedHandler : IEventHandler<RoleCreated>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleCreatedHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
                throw new Exception(GetErrors(identityResult));
        }

        private string GetErrors(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }
    }
}
