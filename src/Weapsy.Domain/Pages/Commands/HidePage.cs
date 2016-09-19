using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Pages.Commands
{
    public class HidePage : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
