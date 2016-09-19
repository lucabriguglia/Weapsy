using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Sites.Events;
using System;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class ReopenSiteTests
    {
        [Test]
        public void Should_throw_exception_when_already_active()
        {
            var site = new Site();

            site.Reopen();
            
            Assert.Throws<Exception>(() => site.Reopen());
        }

        [Test]
        public void Should_throw_exception_when_deleted()
        {
            var site = new Site();

            site.Delete();

            Assert.Throws<Exception>(() => site.Reopen());
        }

        [Test]
        public void Should_set_site_status_to_active()
        {
            var site = new Site();

            site.Reopen();

            Assert.AreEqual(true, site.Status == SiteStatus.Active);
        }

        [Test]
        public void Should_add_site_reopened_event()
        {
            var site = new Site();

            site.Reopen();

            var @event = site.Events.OfType<SiteReopened>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_site_reopened_event()
        {
            var site = new Site();

            site.Reopen();

            var @event = site.Events.OfType<SiteReopened>().SingleOrDefault();

            Assert.AreEqual(site.Id, @event.AggregateRootId);
        }
    }
}
