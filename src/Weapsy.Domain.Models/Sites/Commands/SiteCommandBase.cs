using Kledex.Commands;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public abstract class SiteCommandBase : Command
    {
        public string Name { get; set; }
    }
}