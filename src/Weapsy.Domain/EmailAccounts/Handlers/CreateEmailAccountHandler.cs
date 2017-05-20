using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class CreateEmailAccountHandler : ICommandHandler<CreateEmailAccount>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<CreateEmailAccount> _validator;

        public CreateEmailAccountHandler(IEmailAccountRepository emailAccountRepository,
            IValidator<CreateEmailAccount> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateEmailAccount command)
        {
            var emailAccount = EmailAccount.CreateNew(command, _validator);

            _emailAccountRepository.Create(emailAccount);

            return emailAccount.Events;
        }
    }
}
