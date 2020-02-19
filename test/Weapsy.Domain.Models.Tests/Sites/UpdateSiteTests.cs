using System;
using NUnit.Framework;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Models.Tests.Sites
{
    [TestFixture]
    public class UpdateSiteTests
    {
        private Site _sut;
        private UpdateSite _command;

        [SetUp]
        public void SetUp()
        {
            _command = new UpdateSite
            {
                SiteId = Guid.NewGuid(),
                Name = "My Site"
            };

            _sut = new Site();

            _sut.Update(_command);
        }

        [Test]
        public void SetsName()
        {
            Assert.AreEqual(_command.Name, _sut.Name);
        }
    }
}