using System;
using AutoMapper;

namespace Weapsy.Core.Dispatcher
{
    public static class EventFactory
    {
        public static dynamic GetConcreteEvent(object @event)
        {
            Type type = @event.GetType();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap(type, type); });
            IMapper mapper = config.CreateMapper();
            dynamic result = mapper.Map(@event, type, type);
            return result;
        }
    }
}
