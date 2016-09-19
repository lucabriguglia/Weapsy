using FluentValidation;
using System;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Events;

namespace Weapsy.Domain.Model.Apps
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

        public App() { }

        private App(CreateApp cmd) : base(cmd.Id)
        {
            Name = cmd.Name;
            Description = cmd.Description;
            Folder = cmd.Folder;

            AddEvent(new AppCreated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                Folder = Folder
            });
        }

        public static App CreateNew(CreateApp cmd, IValidator<CreateApp> validator)
        {
            validator.ValidateCommand(cmd);

            return new App(cmd);
        }

        public void UpdateDetails(UpdateAppDetails cmd, IValidator<UpdateAppDetails> validator)
        {
            validator.ValidateCommand(cmd);

            Name = cmd.Name;
            Description = cmd.Description;
            Folder = cmd.Folder;

            AddEvent(new AppDetailsUpdated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                Folder = Folder
            });
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}
