using System;
using Moq;
using NUnit.Framework;
using Weapsy.Core.Domain;
using Weapsy.Core.DependencyResolver;
using Weapsy.Core.Dispatcher;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Weapsy.Core.Tests.Dispatcher
{
    [TestFixture]
    public class CommandSenderTests
    {
        [Test]
        public void Should_throw_an_exception_when_command_is_null()
        {
            var resolverMock = new Mock<IResolver>();
            var eventPublisherMock = new Mock<IEventPublisher>();
            var eventStoreMock = new Mock<IEventStore>();

            var commandSender = new CommandSender(resolverMock.Object, 
                eventPublisherMock.Object, 
                eventStoreMock.Object);

            Assert.Throws<ArgumentNullException>(() => commandSender.Send<ICommand, IAggregateRoot>(null));
        }

        [Test]
        public void Should_throw_an_exception_when_command_handler_is_not_found()
        {
            var resolverMock = new Mock<IResolver>();
            var eventPublisherMock = new Mock<IEventPublisher>();
            var eventStoreMock = new Mock<IEventStore>();

            var commandSender = new CommandSender(resolverMock.Object,
                eventPublisherMock.Object,
                eventStoreMock.Object);

            Assert.Throws<Exception>(() => commandSender.Send<ICommand, IAggregateRoot>(new FakeCommand()));
        }

        [Ignore("Failing due to an async method, it needs investigation.")]
        [Test]
        public void Should_publish_events()
        {
            var fakeCommand = new FakeCommand();
            var fakeEvent = new FakeEvent();

            var commandHandlerMock = new Mock<ICommandHandler<ICommand>>();
            commandHandlerMock.Setup(x => x.Handle(fakeCommand)).Returns(new List<IEvent> { fakeEvent });

            var resolverMock = new Mock<IResolver>();
            resolverMock.Setup(x => x.Resolve<ICommandHandler<ICommand>>()).Returns(commandHandlerMock.Object);

            var eventPublisherMock = new Mock<IEventPublisher>();
            eventPublisherMock.Setup(x => x.Publish(fakeEvent));

            var eventStoreMock = new Mock<IEventStore>();

            var commandSender = new CommandSender(resolverMock.Object,
                eventPublisherMock.Object,
                eventStoreMock.Object);

            commandSender.Send<ICommand, IAggregateRoot>(fakeCommand);

            commandHandlerMock.Verify(x => x.Handle(fakeCommand), Times.Once);
            eventPublisherMock.Verify(x => x.Publish(It.IsAny<IEvent>()), Times.Once);
        }

        private class FakeCommand : ICommand { public Guid Id { get; set; } }
        private class FakeEvent : Event {}
    }
}
