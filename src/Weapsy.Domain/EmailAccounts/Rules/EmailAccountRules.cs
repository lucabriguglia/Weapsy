using System;

namespace Weapsy.Domain.EmailAccounts.Rules
{
    public class EmailAccountRules : IEmailAccountRules
    {
        private readonly IEmailAccountRepository _emailAccountRepository;

        public EmailAccountRules(IEmailAccountRepository emailAccountRepository)
        {
            _emailAccountRepository = emailAccountRepository;
        }

        public bool DoesEmailAccountExist(Guid siteId, Guid id)
        {
            var emailAccount = _emailAccountRepository.GetById(siteId, id);
            return emailAccount != null && emailAccount.Status != EmailAccountStatus.Deleted;
        }

        public bool IsEmailAccountIdUnique(Guid id)
        {
            return _emailAccountRepository.GetById(id) == null;
        }

        public bool IsEmailAccountAddressUnique(Guid siteId, string name, Guid emailAccountId = new Guid())
        {
            var emailAccount = _emailAccountRepository.GetByAddress(siteId, name);
            return emailAccount == null
                || emailAccount.Status == EmailAccountStatus.Deleted
                || (emailAccountId != Guid.Empty && emailAccount.Id == emailAccountId);
        }
    }
}
