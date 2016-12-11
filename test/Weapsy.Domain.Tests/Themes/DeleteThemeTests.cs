using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Events;
using System;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class DeleteThemeTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var theme = new Theme();

            theme.Delete();
            
            Assert.Throws<Exception>(() => theme.Delete());
        }

        [Test]
        public void Should_set_theme_status_to_deleted()
        {
            var theme = new Theme();

            theme.Delete();

            Assert.AreEqual(true, theme.Status == ThemeStatus.Deleted);
        }

        [Test]
        public void Should_add_theme_deleted_event()
        {
            var theme = new Theme();

            theme.Delete();

            var @event = theme.Events.OfType<ThemeDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_deleted_event()
        {
            var theme = new Theme();

            theme.Delete();

            var @event = theme.Events.OfType<ThemeDeleted>().SingleOrDefault();

            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }
    }
}
