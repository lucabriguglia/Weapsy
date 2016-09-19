using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Commands
{
    public class DeleteLanguage : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
