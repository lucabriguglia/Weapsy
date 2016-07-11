using System;
using Weapsy.Core.Domain;
using Weapsy.Core.DependencyResolver;
using AutoMapper;

namespace Weapsy.Core.Dispatcher
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
                _eventStore.SaveEvent<TAggregate>(@event);

                if (!publishEvents)
                    continue;

                // hack for autofac resolver inside event publisher
                Type type = @event.GetType();
                var config = new MapperConfiguration(cfg => { cfg.CreateMap(type, type); });
                IMapper mapper = config.CreateMapper();
                dynamic newEvent = mapper.Map(@event, type, type);

                _eventPublisher.Publish(newEvent);
            }
        }
    }
}
