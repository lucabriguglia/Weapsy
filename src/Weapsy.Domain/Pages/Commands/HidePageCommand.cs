using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class HidePageCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
