using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Users;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Handlers;

namespace Weapsy.Domain.Tests.Users.Handlers
{
    [TestFixture]
    public class CreateUserHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };

            var userRepositoryMock = new Mock<IUserRepository>();

            var validatorMock = new Mock<IValidator<CreateUser>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createUserHandler = new CreateUserHandler(userRepositoryMock.Object, validatorMock.Object);

            Assert.ThrowsAsync<Exception>(async () => await createUserHandler.HandleAsync(command));
        }

        [Test]
        public async Task Should_validate_command_and_save_new_user()
        {
            var command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(false));

            var validatorMock = new Mock<IValidator<CreateUser>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createUserHandler = new CreateUserHandler(userRepositoryMock.Object, validatorMock.Object);
            await createUserHandler.HandleAsync(command);

            validatorMock.Verify(x => x.Validate(command));
            userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()));
        }
    }
}
