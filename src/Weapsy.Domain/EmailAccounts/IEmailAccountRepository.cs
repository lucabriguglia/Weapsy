using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.EmailAccounts
{
    public interface IEmailAccountRepository : IRepository<EmailAccount>
    {
        EmailAccount GetById(Guid id);
        EmailAccount GetById(Guid siteId, Guid id);
        EmailAccount GetByAddress(Guid siteId, string address);
        ICollection<EmailAccount> GetAll(Guid siteId);       
        void Create(EmailAccount language);
        void Update(EmailAccount language);
    }
}
