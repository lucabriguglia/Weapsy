using System;

namespace Weapsy.Domain.EmailAccounts.Rules
{
    public interface IEmailAccountRules
    {
        bool DoesEmailAccountExist(Guid siteId, Guid id);
        bool IsEmailAccountIdUnique(Guid id);
        bool IsEmailAccountAddressUnique(Guid siteId, string name, Guid emailAccountId = new Guid());
    }
}
