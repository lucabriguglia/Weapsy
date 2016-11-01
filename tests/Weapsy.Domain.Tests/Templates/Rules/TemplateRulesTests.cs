using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Rules;

namespace Weapsy.Domain.Tests.Templates.Rules
{
    [TestFixture]
    public class TemplateRulesTests
    {
        [Test]
        public void Should_return_false_if_template_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Template());

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_template_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Template)null);

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_template_name_is_not_unique()
        {
            var name = "My Template";

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns(new Template());

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateNameUnique(name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_template_name_is_unique()
        {
            var name = "My Template";

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns((Template)null);

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateNameUnique(name);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("a b")]
        [TestCase("")]
        public void Should_return_false_if_template_view_name_is_not_valid(string viewName)
        {
            var sut = new TemplateRules(new Mock<ITemplateRepository>().Object);
            var actual = sut.IsTemplateViewNameValid(viewName);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        public void Should_return_true_if_template_view_name_is_valid(string viewName)
        {
            var sut = new TemplateRules(new Mock<ITemplateRepository>().Object);
            var actual = sut.IsTemplateViewNameValid(viewName);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_template_view_name_is_not_unique()
        {
            var viewName = "my-template";

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetByViewName(viewName)).Returns(new Template());

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateViewNameUnique(viewName);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_template_view_name_is_unique()
        {
            var viewName = "my-template";

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetByViewName( viewName)).Returns((Template)null);

            var sut = new TemplateRules(repositoryMock.Object);

            var actual = sut.IsTemplateViewNameUnique(viewName);

            Assert.AreEqual(true, actual);
        }
    }
}
