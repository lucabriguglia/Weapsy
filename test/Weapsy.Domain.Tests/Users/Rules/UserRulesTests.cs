using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Users;
using Weapsy.Domain.Users.Rules;

namespace Weapsy.Domain.Tests.Users.Rules
{
    [TestFixture]
    public class UserRulesTests
    {
        [Test]
        public void Should_return_false_if_user_name_is_not_unique()
        {
            var userName = "MyUser";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByUserName(userName)).Returns(new User());

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserNameUnique(userName);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_user_name_is_unique()
        {
            var userName = "MyUser";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByUserName(userName)).Returns((User)null);

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserNameUnique(userName);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_user_email_is_not_unique()
        {
            var email = "my@email.com";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(email)).Returns(new User());

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserEmailUnique(email);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_user_email_is_unique()
        {
            var email = "my@email.com";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(email)).Returns((User)null);

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserEmailUnique(email);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_user_email_is_not_unique_for_existing_users()
        {
            var userId = Guid.NewGuid();
            var email = "my@email.com";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(email)).Returns(new User());

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserEmailUnique(email, userId);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_user_url_is_unique_for_existing_users()
        {
            var userId = Guid.NewGuid();
            var email = "my@email.com";

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(email)).Returns((User)null);

            var sut = new UserRules(repositoryMock.Object);

            var actual = sut.IsUserEmailUnique(email, userId);

            Assert.AreEqual(true, actual);
        }
    }
}
