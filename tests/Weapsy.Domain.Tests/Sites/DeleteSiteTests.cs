using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Events;
using System;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class DeleteSiteTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var site = new Site();

            site.Delete();
            
            Assert.Throws<Exception>(() => site.Delete());
        }

        [Test]
        public void Should_set_site_status_to_deleted()
        {
            var site = new Site();

            site.Delete();

            Assert.AreEqual(true, site.Status == SiteStatus.Deleted);
        }

        [Test]
        public void Should_add_site_deleted_event()
        {
            var site = new Site();

            site.Delete();

            var @event = site.Events.OfType<SiteDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_site_deleted_event()
        {
            var site = SiteFactory.CreateNew();

            site.Delete();

            var @event = site.Events.OfType<SiteDeleted>().SingleOrDefault();

            Assert.AreEqual(site.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_site_deleted_event()
        {
            var site = SiteFactory.CreateNew();

            site.Delete();

            var @event = site.Events.OfType<SiteDeleted>().SingleOrDefault();

            Assert.AreEqual(site.Name, @event.Name);
        }
    }
}
