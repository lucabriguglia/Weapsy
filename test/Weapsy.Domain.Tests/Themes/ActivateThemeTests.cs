using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Events;
using System;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class ActivateThemeTests
    {
        [Test]
        public void Should_throw_exception_when_already_activated()
        {
            var theme = new Theme();

            theme.Activate();
            
            Assert.Throws<Exception>(() => theme.Activate());
        }

        [Test]
        public void Should_set_theme_status_to_activated()
        {
            var theme = new Theme();

            theme.Activate();

            Assert.AreEqual(true, theme.Status == ThemeStatus.Active);
        }

        [Test]
        public void Should_add_theme_activated_event()
        {
            var theme = new Theme();

            theme.Activate();

            var @event = theme.Events.OfType<ThemeActivated>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_activated_event()
        {
            var theme = new Theme();

            theme.Activate();

            var @event = theme.Events.OfType<ThemeActivated>().SingleOrDefault();

            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }
    }
}
