using System;
using NUnit.Framework;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Models.Tests.Sites
{
    [TestFixture]
    public class CreateSiteTests
    {
        private Site _sut;
        private CreateSite _command;

        [SetUp]
        public void SetUp()
        {
            _command = new CreateSite
            {
                SiteId = Guid.NewGuid(),
                Name = "My Site"
            };

            _sut = new Site(_command);
        }

        [Test]
        public void SetsId()
        {
            Assert.AreEqual(_command.SiteId, _sut.Id);
        }

        [Test]
        public void SetsName()
        {
            Assert.AreEqual(_command.Name, _sut.Name);
        }
    }
}
