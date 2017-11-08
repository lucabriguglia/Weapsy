using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class DeletePageCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
