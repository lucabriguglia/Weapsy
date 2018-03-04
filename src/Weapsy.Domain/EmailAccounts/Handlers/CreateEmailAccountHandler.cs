using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.EmailAccounts.Commands;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class CreateEmailAccountHandler : ICommandHandlerWithAggregate<CreateEmailAccount>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<CreateEmailAccount> _validator;

        public CreateEmailAccountHandler(IEmailAccountRepository emailAccountRepository,
            IValidator<CreateEmailAccount> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateEmailAccount command)
        {
            var emailAccount = EmailAccount.CreateNew(command, _validator);

            _emailAccountRepository.Create(emailAccount);

            return emailAccount;
        }
    }
}
