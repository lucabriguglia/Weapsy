using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EmailAccounts.Rules
{
    public interface IEmailAccountRules : IRules<EmailAccount>
    {
        bool DoesEmailAccountExist(Guid siteId, Guid id);
        bool IsEmailAccountIdUnique(Guid id);
        bool IsEmailAccountAddressUnique(Guid siteId, string name, Guid emailAccountId = new Guid());
    }
}
