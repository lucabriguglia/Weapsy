using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.EmailAccounts.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.EmailAccounts.Handlers
{
    public class UpdateEmailAccountDetailsHandler : ICommandHandler<UpdateEmailAccountDetailsCommand>
    {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IValidator<UpdateEmailAccountDetailsCommand> _validator;

        public UpdateEmailAccountDetailsHandler(IEmailAccountRepository emailAccountRepository, IValidator<UpdateEmailAccountDetailsCommand> validator)
        {
            _emailAccountRepository = emailAccountRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateEmailAccountDetailsCommand command)
        {
            var emailAccount = _emailAccountRepository.GetById(command.SiteId, command.Id);

            if (emailAccount == null)
                throw new Exception("Email Account not found.");

            emailAccount.UpdateDetails(command, _validator);

            _emailAccountRepository.Update(emailAccount);

            return emailAccount.Events;
        }
    }
}
