using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Reporting.EmailAccounts;

namespace Weapsy.Reporting.Data.Default.EmailAccounts
{
    public class EmailAccountFacade : IEmailAccountFacade
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public EmailAccountFacade(IEmailAccountRepository emailAccountRepository, 
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _emailAccountRepository = emailAccountRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<EmailAccountModel> GetAll(Guid siteId)
        {
            var emailAccounts = _emailAccountRepository.GetAll(siteId);
            return _mapper.Map<IEnumerable<EmailAccountModel>>(emailAccounts);
        }

        public EmailAccountModel Get(Guid siteId, Guid id)
        {
            var emailAccount = _emailAccountRepository.GetById(siteId, id);
            return emailAccount == null ? null : _mapper.Map<EmailAccountModel>(emailAccount);
        }
    }
}