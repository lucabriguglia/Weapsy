using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.DependencyResolver;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Dispatcher
{
    public class CommandSender : ICommandSender
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher,
            IEventStore eventStore)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        public void Send<TCommand, TAggregate>(TCommand command, bool publishEvents = true)
            where TCommand : ICommand
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (commandHandler == null)
                throw new Exception($"No handler found for command '{command.GetType().FullName}'");

            var events = commandHandler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);

                if (concreteEvent is IDomainEvent)
                {
                    _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent);
                }
                
                if (!publishEvents)
                    continue;

                _eventPublisher.Publish(concreteEvent);
            }
        }

        public async Task SendAsync<TCommand, TAggregate>(TCommand command, bool publishEvents = true) 
            where TCommand : ICommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandlerAsync<TCommand>>();

            if (commandHandler == null)
                throw new Exception($"No handler found for command '{command.GetType().FullName}'");

            var events = await commandHandler.HandleAsync(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);

                if (concreteEvent is IDomainEvent)
                {
                    await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);
                }
                
                if (!publishEvents)
                    continue;

                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }
    }
}
