using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Events;
using System;

namespace Weapsy.Domain.Tests.Templates
{
    [TestFixture]
    public class ActivateTemplateTests
    {
        [Test]
        public void Should_throw_exception_when_already_activated()
        {
            var template = new Template();

            template.Activate();
            
            Assert.Throws<Exception>(() => template.Activate());
        }

        [Test]
        public void Should_set_template_status_to_activated()
        {
            var template = new Template();

            template.Activate();

            Assert.AreEqual(true, template.Status == TemplateStatus.Active);
        }

        [Test]
        public void Should_add_template_activated_event()
        {
            var template = new Template();

            template.Activate();

            var @event = template.Events.OfType<TemplateActivated>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_template_activated_event()
        {
            var template = new Template();

            template.Activate();

            var @event = template.Events.OfType<TemplateActivated>().SingleOrDefault();

            Assert.AreEqual(template.Id, @event.AggregateRootId);
        }
    }
}
