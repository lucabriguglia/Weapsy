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
    public class UpdateEmailAccountDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_email_account_is_not_found()
        {
            var command = new UpdateEmailAccountDetails
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
            emailAccountRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((EmailAccounts.EmailAccount)null);

            var validatorMock = new Mock<IValidator<UpdateEmailAccountDetails>>();

            var createEmailAccountHandler = new UpdateEmailAccountDetailsHandler(emailAccountRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createEmailAccountHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateEmailAccountDetails
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
            emailAccountRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new EmailAccounts.EmailAccount());

            var validatorMock = new Mock<IValidator<UpdateEmailAccountDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Address", "Address Error") }));

            var createEmailAccountHandler = new UpdateEmailAccountDetailsHandler(emailAccountRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createEmailAccountHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_email_account()
        {
            var command = new UpdateEmailAccountDetails
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
            emailAccountRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new EmailAccounts.EmailAccount());
            emailAccountRepositoryMock.Setup(x => x.Update(It.IsAny<EmailAccounts.EmailAccount>()));

            var validatorMock = new Mock<IValidator<UpdateEmailAccountDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createEmailAccountHandler = new UpdateEmailAccountDetailsHandler(emailAccountRepositoryMock.Object, validatorMock.Object);
            createEmailAccountHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            emailAccountRepositoryMock.Verify(x => x.Update(It.IsAny<EmailAccounts.EmailAccount>()));
        }
    }
}
