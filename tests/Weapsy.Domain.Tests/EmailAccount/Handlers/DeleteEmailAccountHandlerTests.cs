using System;
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
    public class DeleteEmailAccountHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_email_account_is_not_found()
        {
            var command = new DeleteEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((EmailAccounts.EmailAccount)null);

            var validatorMock = new Mock<IValidator<DeleteEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var deleteEmailAccountHandler = new DeleteEmailAccountHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => deleteEmailAccountHandler.Handle(command));
        }

        [Test]
        public void Should_update_emailAccount()
        {
            var command = new DeleteEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var emailAccountMock = new Mock<EmailAccounts.EmailAccount>();

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(emailAccountMock.Object);

            var validatorMock = new Mock<IValidator<DeleteEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var deleteEmailAccountHandler = new DeleteEmailAccountHandler(repositoryMock.Object, validatorMock.Object);

            deleteEmailAccountHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<EmailAccounts.EmailAccount>()));
        }
    }
}
