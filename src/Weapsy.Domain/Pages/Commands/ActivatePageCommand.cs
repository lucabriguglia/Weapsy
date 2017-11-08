using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class ActivatePageCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
