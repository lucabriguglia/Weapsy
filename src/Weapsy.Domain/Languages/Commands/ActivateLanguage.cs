using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Languages.Commands
{
    public class ActivateLanguage : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
