using System;
using System.Collections.Generic;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.EmailAccounts;
using System.Linq;

namespace Weapsy.Reporting.Data.EmailAccounts
{
    public class EmailAccountFacade : IEmailAccountFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public EmailAccountFacade(IDbContextFactory dbContextFactory, 
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<EmailAccountModel> GetAll(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.EmailAccounts
                    .Where(x => x.SiteId == siteId && x.Status != EmailAccountStatus.Deleted)
                    .ToList();

                return _mapper.Map<IEnumerable<EmailAccountModel>>(dbEntities);
            }
        }

        public EmailAccountModel Get(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.EmailAccounts.FirstOrDefault(x => x.SiteId == siteId && x.Id == id && x.Status != EmailAccountStatus.Deleted);
                return dbEntity != null ? _mapper.Map<EmailAccountModel>(dbEntity) : null;
            }
        }
    }
}