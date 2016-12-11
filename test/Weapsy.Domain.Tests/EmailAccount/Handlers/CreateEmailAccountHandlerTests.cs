using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Handlers;

namespace Weapsy.Domain.Tests.EmailAccount.Handlers
{
    [TestFixture]
    public class CreateEmailAccountHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Address = "info@mysite.com",
                DisplayName = "My Site",
                Host = "host",
                Port = 25,
                UserName = "username",
                Password = "password",
                DefaultCredentials = true,
                Ssl = false
            };

            var emailAccountRepositoryMock = new Mock<IEmailAccountRepository>();

            var validatorMock = new Mock<IValidator<CreateEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createEmailAccountHandler = new CreateEmailAccountHandler(emailAccountRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createEmailAccountHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_email_account()
        {
            var command = new CreateEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Address = "info@mysite.com",
                DisplayName = "My Site",
                Host = "host",
                Port = 25,
                UserName = "username",
                Password = "password",
                DefaultCredentials = true,
                Ssl = false
            };

            var emailAccountRepositoryMock = new Mock<IEmailAccountRepository>();
            emailAccountRepositoryMock.Setup(x => x.Create(It.IsAny<EmailAccounts.EmailAccount>()));

            var validatorMock = new Mock<IValidator<CreateEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createEmailAccountHandler = new CreateEmailAccountHandler(emailAccountRepositoryMock.Object, validatorMock.Object);
            createEmailAccountHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            emailAccountRepositoryMock.Verify(x => x.Create(It.IsAny<EmailAccounts.EmailAccount>()));
        }
    }
}
