using System;
using Weapsy.Core.Domain;

namespace Weapsy.Apps.Text.Domain.Commands
{
    public class CreateTextModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}
