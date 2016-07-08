using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Commands
{
    public class CreateRole : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
