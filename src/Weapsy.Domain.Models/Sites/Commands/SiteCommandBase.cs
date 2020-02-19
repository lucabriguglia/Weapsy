using Weapsy.Core;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public class SiteCommandBase : Command
    {
        public string Name { get; set; }
    }
}
