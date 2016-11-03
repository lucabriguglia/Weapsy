using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class DeletePage : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
