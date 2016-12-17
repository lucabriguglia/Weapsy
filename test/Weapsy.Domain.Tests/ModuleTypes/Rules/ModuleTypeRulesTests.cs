using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.Tests.ModuleTypes.Rules
{
    [TestFixture]
    public class ModuleTypeRulesTests
    {
        [Test]
        public void Should_return_false_if_module_type_does_not_exist()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetById(id)).Returns((ModuleType)null);

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.DoesModuleTypeExist(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_type_does_exist()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetById(id)).Returns(new ModuleType());

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.DoesModuleTypeExist(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_module_type_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetById(id)).Returns(new ModuleType());

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_type_id_is_unique()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetById(id)).Returns((ModuleType)null);

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("")]
        public void Should_return_false_if_module_type_name_is_not_valid(string name)
        {
            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);
            var actual = sut.IsModuleTypeNameValid(name);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a b")]
        public void Should_return_true_if_module_type_name_is_valid(string name)
        {
            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);
            var actual = sut.IsModuleTypeNameValid(name);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_module_type_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Module Type";

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetByName(name)).Returns(new ModuleType());

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeNameUnique(name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_type_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Module Type";

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetByName(name)).Returns((ModuleType)null);

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeNameUnique(name);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_module_type_view_component_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var viewComponentName = "ViewComponentName";

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetByViewComponentName(viewComponentName)).Returns(new ModuleType());

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeViewComponentNameUnique(viewComponentName);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_type_view_component_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var viewComponentName = "ViewComponentName";

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            moduleTypeRepositoryMock.Setup(x => x.GetByViewComponentName(viewComponentName)).Returns((ModuleType)null);

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeViewComponentNameUnique(viewComponentName);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_module_type_is_not_in_use()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleTypeId(id)).Returns(0);

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeInUse(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_type_is_in_use()
        {
            var id = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleTypeId(id)).Returns(10);

            var sut = new ModuleTypeRules(moduleTypeRepositoryMock.Object, moduleRepositoryMock.Object);

            var actual = sut.IsModuleTypeInUse(id);

            Assert.AreEqual(true, actual);
        }
    }
}
