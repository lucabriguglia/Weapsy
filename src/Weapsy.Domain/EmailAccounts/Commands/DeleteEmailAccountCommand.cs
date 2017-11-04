using System;

namespace Weapsy.Domain.EmailAccounts.Commands
{
    public class DeleteEmailAccountCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
