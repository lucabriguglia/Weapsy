using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class DeleteEmailAccountHandler : ICommandHandler<DeleteEmailAccountCommand>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<DeleteEmailAccountCommand> _validator;

        public DeleteEmailAccountHandler(IEmailAccountRepository emailAccountRepository, IValidator<DeleteEmailAccountCommand> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(DeleteEmailAccountCommand command)
        {
            var emailAccount = _emailAccountRepository.GetById(command.SiteId, command.Id);

            if (emailAccount == null)
                throw new Exception("Email Account not found.");

            emailAccount.Delete(command, _validator);

            _emailAccountRepository.Update(emailAccount);

            return emailAccount.Events;
        }
    }
}
