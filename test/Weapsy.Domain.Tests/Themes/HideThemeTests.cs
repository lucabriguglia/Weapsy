using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Events;
using System;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class HideThemeTests
    {
        [Test]
        public void Should_throw_exception_when_already_hidden()
        {
            var theme = new Theme();

            theme.Hide();

            Assert.Throws<Exception>(() => theme.Hide());
        }

        [Test]
        public void Should_set_theme_status_to_hidden()
        {
            var theme = new Theme();

            theme.Hide();

            Assert.AreEqual(true, theme.Status == ThemeStatus.Hidden);
        }

        [Test]
        public void Should_add_theme_hidden_event()
        {
            var theme = new Theme();

            theme.Hide();

            var @event = theme.Events.OfType<ThemeHidden>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_hidden_event()
        {
            var theme = new Theme();

            theme.Hide();

            var @event = theme.Events.OfType<ThemeHidden>().SingleOrDefault();

            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }
    }
}
