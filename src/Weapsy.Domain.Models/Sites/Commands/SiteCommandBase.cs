using Weapsy.Core.Domain;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public class SiteCommandBase : Command
    {
        public string Name { get; set; }
    }
}
