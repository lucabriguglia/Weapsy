using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class ActivatePage : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
