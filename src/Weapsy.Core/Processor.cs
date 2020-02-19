using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Core
{
    public class Processor : IProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public Processor(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task ProcessAsync(Func<Task<CommandResponse>> action)
        {
            var response = await action();

            if (response.Events.Count > 0)
            {
                var tasks = new List<Task>();

                foreach (var @event in response.Events)
                {
                    var eventType = @event.GetType();
                    dynamic concreteEvent = _mapper.Map(@event, eventType, eventType);
                    tasks.AddRange(GetTasks(concreteEvent));
                }

                await Task.WhenAll(tasks);
            }
        }

        private IEnumerable<Task> GetTasks<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            var eventHandlers = handlers as IEventHandler<TEvent>[] ?? handlers.ToArray();
            return eventHandlers.Any() 
                ? eventHandlers.Select(handler => handler.HandleAsync(@event)).ToList() 
                : new List<Task>();
        }
    }
}