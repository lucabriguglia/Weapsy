using System;
using Weapsy.Core.Domain;
using Weapsy.Core.DependencyResolver;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Weapsy.Core.Dispatcher
{
    public class CommandSender : ICommandSender
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;
        private readonly ILogger _logger;

        public CommandSender(IResolver resolver, 
            IEventPublisher eventPublisher, 
            IEventStore eventStore,
            ILoggerFactory loggerFactory)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _logger = loggerFactory.CreateLogger<CommandSender>();
        }

        public void Send<TCommand, TAggregate>(TCommand command, bool publishEvents = true) 
            where TCommand : ICommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
            {
                _logger.LogError($"{nameof(command)} is null.");
                throw new ArgumentNullException(nameof(command));
            }
                
            var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (commandHandler == null)
            {
                var errorMessage = $"No handler found for command '{command.GetType().FullName}'";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            ICollection<IEvent> events = new List<IEvent>();

            try
            {
                events = commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
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
