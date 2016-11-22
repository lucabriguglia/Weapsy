using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Infrastructure.DependencyResolver;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Tests.Dispatcher
{
    [TestFixture]
    public class EventPublisherTests
    {
        [Test]
        public void Should_throw_an_exception_when_event_is_null()
        {
            var resolverMock = new Mock<IResolver>();
            var eventPublisher = new EventPublisher(resolverMock.Object);

            Assert.That(() => eventPublisher.Publish<IEvent>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Should_handle_events()
        {
            var fakeEvent = new FakeEvent();

            var eventHandler1Mock = new Mock<IEventHandlerAsync<IEvent>>();
            eventHandler1Mock.Setup(x => x.HandleAsync(fakeEvent));

            var eventHandler2Mock = new Mock<IEventHandlerAsync<IEvent>>();
            eventHandler2Mock.Setup(x => x.HandleAsync(fakeEvent));

            var eventHandlers = new List<IEventHandlerAsync<IEvent>> { eventHandler1Mock.Object, eventHandler2Mock.Object };

            var resolverMock = new Mock<IResolver>();
            resolverMock.Setup(x => x.ResolveAll<IEventHandlerAsync<IEvent>>()).Returns(eventHandlers);

            var eventPublisher = new EventPublisher(resolverMock.Object);

            eventPublisher.Publish<IEvent>(fakeEvent);

            eventHandler1Mock.Verify(x => x.HandleAsync(fakeEvent), Times.Once);
            eventHandler2Mock.Verify(x => x.HandleAsync(fakeEvent), Times.Once);
        }

        private class FakeEvent : Event {}
    }
}
