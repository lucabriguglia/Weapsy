using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EmailAccounts.Commands
{
    public class DeleteEmailAccount : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
