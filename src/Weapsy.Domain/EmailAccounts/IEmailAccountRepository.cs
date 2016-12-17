using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EmailAccounts
{
    public interface IEmailAccountRepository : IRepository<EmailAccount>
    {
        EmailAccount GetById(Guid id);
        EmailAccount GetById(Guid siteId, Guid id);
        EmailAccount GetByAddress(Guid siteId, string address);    
        void Create(EmailAccount language);
        void Update(EmailAccount language);
    }
}
