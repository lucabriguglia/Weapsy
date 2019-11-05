using Kledex.Domain;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public class CreateSite : DomainCommand<Site>
    {
        public string Name { get; set; }
    }
}
