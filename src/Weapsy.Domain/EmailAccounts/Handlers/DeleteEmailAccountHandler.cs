using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.EmailAccounts.Commands;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class DeleteEmailAccountHandler : ICommandHandlerWithAggregate<DeleteEmailAccount>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<DeleteEmailAccount> _validator;

        public DeleteEmailAccountHandler(IEmailAccountRepository emailAccountRepository, IValidator<DeleteEmailAccount> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(DeleteEmailAccount command)
        {
            var emailAccount = _emailAccountRepository.GetById(command.SiteId, command.Id);

            if (emailAccount == null)
                throw new Exception("Email Account not found.");

            emailAccount.Delete(command, _validator);

            _emailAccountRepository.Update(emailAccount);

            return emailAccount;
        }
    }
}
