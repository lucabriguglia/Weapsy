using System;

namespace Weapsy.Domain.EmailAccounts
{
    public interface IEmailAccountRepository
    {
        EmailAccount GetById(Guid id);
        EmailAccount GetById(Guid siteId, Guid id);
        EmailAccount GetByAddress(Guid siteId, string address);    
        void Create(EmailAccount language);
        void Update(EmailAccount language);
    }
}
