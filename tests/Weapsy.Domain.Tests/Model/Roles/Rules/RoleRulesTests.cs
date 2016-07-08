using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Roles;
using Weapsy.Domain.Model.Roles.Rules;

namespace Weapsy.Domain.Tests.Roles.Handlers
{
    [TestFixture]
    public class RoleRulesTests
    {
        [Test]
        public void Should_return_false_if_role_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IRoleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Role());

            var sut = new RoleRules(repositoryMock.Object);

            var actual = sut.IsRoleIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_role_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IRoleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Role)null);

            var sut = new RoleRules(repositoryMock.Object);

            var actual = sut.IsRoleIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_role_name_is_not_unique()
        {
            var roleName = "MyRole";

            var repositoryMock = new Mock<IRoleRepository>();
            repositoryMock.Setup(x => x.GetByName(roleName)).Returns(new Role());

            var sut = new RoleRules(repositoryMock.Object);

            var actual = sut.IsRoleNameUnique(roleName);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_role_name_is_unique()
        {
            var roleName = "MyRole";

            var repositoryMock = new Mock<IRoleRepository>();
            repositoryMock.Setup(x => x.GetByName(roleName)).Returns((Role)null);

            var sut = new RoleRules(repositoryMock.Object);

            var actual = sut.IsRoleNameUnique(roleName);

            Assert.AreEqual(true, actual);
        }
    }
}
