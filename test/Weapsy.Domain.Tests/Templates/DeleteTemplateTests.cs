using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Events;
using System;

namespace Weapsy.Domain.Tests.Templates
{
    [TestFixture]
    public class DeleteTemplateTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var template = new Template();

            template.Delete();
            
            Assert.Throws<Exception>(() => template.Delete());
        }

        [Test]
        public void Should_set_template_status_to_deleted()
        {
            var template = new Template();

            template.Delete();

            Assert.AreEqual(true, template.Status == TemplateStatus.Deleted);
        }

        [Test]
        public void Should_add_template_deleted_event()
        {
            var template = new Template();

            template.Delete();

            var @event = template.Events.OfType<TemplateDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_template_deleted_event()
        {
            var template = new Template();

            template.Delete();

            var @event = template.Events.OfType<TemplateDeleted>().SingleOrDefault();

            Assert.AreEqual(template.Id, @event.AggregateRootId);
        }
    }
}
