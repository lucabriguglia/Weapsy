using NUnit.Framework;
using System;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Models.Tests
{
    [TestFixture]
    public class SiteTests
    {
        private Site _site;
        private CreateSite _command;

        [SetUp]
        public void SetUp()
        {
            _command = new CreateSite
            {
                AggregateRootId = Guid.NewGuid(),
                Name = "My Site"
            };

            _site = new Site(_command);
        }

        [Test]
        public void SetsId()
        {
            Assert.AreEqual(_command.AggregateRootId, _site.Id);
        }

        [Test]
        public void SetsName()
        {
            Assert.AreEqual(_command.Name, _site.Name);
        }
    }
}
