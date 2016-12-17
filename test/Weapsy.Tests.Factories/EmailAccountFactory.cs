using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;

namespace Weapsy.Tests.Factories
{
    public static class EmailAccountFactory
    {
        public static EmailAccount EmailAccount()
        {
            return EmailAccount(Guid.NewGuid(), Guid.NewGuid(), "info@weapsy.org");
        }

        public static EmailAccount EmailAccount(Guid siteId, Guid id, string address)
        {
            var command = new CreateEmailAccount
            {
                SiteId = siteId,
                Id = id,
                Address = address,
            };

            var validatorMock = new Mock<IValidator<CreateEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Domain.EmailAccounts.EmailAccount.CreateNew(command, validatorMock.Object);
        }
    }
}
