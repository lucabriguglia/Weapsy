using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.EmailAccounts
{
    public interface IEmailAccountFacade
    {
        IEnumerable<EmailAccountModel> GetAll(Guid siteId);
        EmailAccountModel Get(Guid siteId, Guid id);
    }
}
