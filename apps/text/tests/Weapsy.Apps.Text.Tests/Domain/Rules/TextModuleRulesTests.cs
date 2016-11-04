using System;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Rules;

namespace Weapsy.Apps.Text.Tests.Domain.Rules
{
    [TestFixture]
    public class TextMdouleRulesTests
    {
        [Test]
        public void Should_return_false_if_text_module_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ITextModuleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new TextModule());

            var sut = new TextModuleRules(repositoryMock.Object);

            var actual = sut.IsTextModuleIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_text_module_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ITextModuleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((TextModule)null);

            var sut = new TextModuleRules(repositoryMock.Object);

            var actual = sut.IsTextModuleIdUnique(id);

            Assert.AreEqual(true, actual);
        }
    }
}
