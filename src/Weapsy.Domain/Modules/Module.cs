using FluentValidation;
using System;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Events;

namespace Weapsy.Domain.Modules
{
    public class Module : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public Guid ModuleTypeId { get; private set; }
        public string Title { get; private set; }
        public ModuleStatus Status { get; private set; }

        //public bool AllPages { get; private set; }
        //public bool InheritPermissions { get; private set; }
        //public string Authorized { get; private set; }
        //public DateTime? StartDate { get; private set; }
        //public DateTime? EndDate { get; private set; }
        //public bool DisplayTitle { get; private set; }
        //public string DefaultZone { get; private set; }

        public Module(){}

        private Module(Guid siteId, Guid moduleTypeId, Guid id, string title) : base(id)
        {
            SiteId = siteId;
            ModuleTypeId = moduleTypeId;
            Title = title;
            Status = ModuleStatus.Active;

            AddEvent(new ModuleCreatedEvent
            {
                SiteId = SiteId,
                ModuleTypeId = ModuleTypeId,
                AggregateRootId = Id,
                Title = Title,
                Status = Status
            });
        }

        public static Module CreateNew(Guid siteId, Guid moduleTypeId, Guid id, string title)
        {
            return new Module(siteId, moduleTypeId, id, title);
        }

        private Module(CreateModuleCommand cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            ModuleTypeId = cmd.ModuleTypeId;
            Title = cmd.Title;
            Status = ModuleStatus.Active;

            AddEvent(new ModuleCreatedEvent
            {
                SiteId = SiteId,
                ModuleTypeId = ModuleTypeId,
                AggregateRootId = Id,
                Title = Title,
                Status = Status
            });
        }

        public static Module CreateNew(CreateModuleCommand cmd, IValidator<CreateModuleCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new Module(cmd);
        }

        public void UpdateDetails()
        {
            throw new NotImplementedException();
        }

        public void Delete(DeleteModuleCommand cmd, IValidator<DeleteModuleCommand> validator)
        {
            if (Status == ModuleStatus.Deleted)
                throw new Exception("Module already deleted");

            validator.ValidateCommand(cmd);

            Status = ModuleStatus.Deleted;

            AddEvent(new ModuleDeletedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == ModuleStatus.Deleted)
                throw new Exception("Module already deleted");

            Status = ModuleStatus.Deleted;

            AddEvent(new ModuleDeletedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}
