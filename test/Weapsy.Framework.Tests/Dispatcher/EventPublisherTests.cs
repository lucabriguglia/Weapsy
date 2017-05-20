using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Framework.DependencyResolver;
using Weapsy.Framework.Domain;
using Weapsy.Framework.Events;

namespace Weapsy.Framework.Tests.Dispatcher
{
    [TestFixture]
    public class EventPublisherTests
    {
        [Test]
        public void Should_throw_an_exception_when_event_is_null()
        {
            var resolverMock = new Mock<IResolver>();
            var eventPublisher = new EventPublisher(resolverMock.Object);

            Assert.That(() => eventPublisher.Publish<IDomainEvent>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Should_handle_events()
        {
            var fakeEvent = new FakeEvent();

            var eventHandler1Mock = new Mock<IEventHandler<IDomainEvent>>();
            eventHandler1Mock.Setup(x => x.Handle(fakeEvent));

            var eventHandler2Mock = new Mock<IEventHandler<IDomainEvent>>();
            eventHandler2Mock.Setup(x => x.Handle(fakeEvent));

            var eventHandlers = new List<IEventHandler<IDomainEvent>> { eventHandler1Mock.Object, eventHandler2Mock.Object };

            var resolverMock = new Mock<IResolver>();
            resolverMock.Setup(x => x.ResolveAll<IEventHandler<IDomainEvent>>()).Returns(eventHandlers);

            var eventPublisher = new EventPublisher(resolverMock.Object);

            eventPublisher.Publish<IDomainEvent>(fakeEvent);

            eventHandler1Mock.Verify(x => x.Handle(fakeEvent), Times.Once);
            eventHandler2Mock.Verify(x => x.Handle(fakeEvent), Times.Once);
        }

        private class FakeEvent : DomainEvent {}
    }
}
