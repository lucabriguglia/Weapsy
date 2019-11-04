using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Events;

// ReSharper disable UnusedMember.Local

namespace Weapsy.Domain.Apps
{
    public class App : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Folder { get; private set; }
        public AppStatus Status { get; private set; }
        
        //public string Url { get; private set; }
        //public string AdminController { get; private set; }
        //public string AdminAction { get; private set; }
        //public string DashboardIcon { get; private set; }    

        public App()
        {
        }

        public App(CreateApp cmd) : base(cmd.Id)
        {
            AddEvent(new AppCreated
            {
                AggregateRootId = Id,
                Name = cmd.Name,
                Description = cmd.Description,
                Folder = cmd.Folder,
                Status = AppStatus.Active
            });
        }

        public void UpdateDetails(UpdateAppDetails cmd)
        {
            AddEvent(new AppDetailsUpdated
            {
                AggregateRootId = Id,
                Name = cmd.Name,
                Description = cmd.Description,
                Folder = cmd.Folder
            });
        }

        private void Apply(AppCreated @event)
        {
            Id = @event.AggregateRootId;
            Name = @event.Name;
            Description = @event.Description;
            Folder = @event.Folder;
            Status = @event.Status;
        }

        private void Apply(AppDetailsUpdated @event)
        {
            Name = @event.Name;
            Description = @event.Description;
            Folder = @event.Folder;
        }
    }
}
