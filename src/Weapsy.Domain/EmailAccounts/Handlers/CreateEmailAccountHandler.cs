using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class CreateEmailAccountHandler : ICommandHandler<CreateEmailAccountCommand>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<CreateEmailAccountCommand> _validator;

        public CreateEmailAccountHandler(IEmailAccountRepository emailAccountRepository,
            IValidator<CreateEmailAccountCommand> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateEmailAccountCommand command)
        {
            var emailAccount = EmailAccount.CreateNew(command, _validator);

            _emailAccountRepository.Create(emailAccount);

            return emailAccount.Events;
        }
    }
}
