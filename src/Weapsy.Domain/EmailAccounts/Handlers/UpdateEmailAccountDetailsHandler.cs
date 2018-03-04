using FluentValidation;
using Weapsy.Domain.EmailAccounts.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class UpdateEmailAccountDetailsHandler : ICommandHandlerWithAggregate<UpdateEmailAccountDetails>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<UpdateEmailAccountDetails> _validator;

        public UpdateEmailAccountDetailsHandler(IEmailAccountRepository emailAccountRepository, IValidator<UpdateEmailAccountDetails> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdateEmailAccountDetails command)
        {
            var emailAccount = _emailAccountRepository.GetById(command.SiteId, command.Id);

            if (emailAccount == null)
                throw new Exception("Email Account not found.");

            emailAccount.UpdateDetails(command, _validator);

            _emailAccountRepository.Update(emailAccount);

            return emailAccount;
        }
    }
}
