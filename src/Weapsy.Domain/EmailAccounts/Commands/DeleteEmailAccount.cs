using System;

namespace Weapsy.Domain.EmailAccounts.Commands
{
    public class DeleteEmailAccount : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
