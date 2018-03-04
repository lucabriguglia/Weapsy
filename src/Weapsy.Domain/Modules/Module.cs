using FluentValidation;
using System;
using Weapsy.Cqrs.Domain;
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
            AddEvent(new ModuleCreated
            {
                SiteId = siteId,
                ModuleTypeId = moduleTypeId,
                AggregateRootId = Id,
                Title = title,
                Status = ModuleStatus.Active
            });
        }

        public static Module CreateNew(Guid siteId, Guid moduleTypeId, Guid id, string title)
        {
            return new Module(siteId, moduleTypeId, id, title);
        }

        private Module(CreateModule cmd) : base(cmd.Id)
        {
            AddEvent(new ModuleCreated
            {
                SiteId = cmd.SiteId,
                ModuleTypeId = cmd.ModuleTypeId,
                AggregateRootId = Id,
                Title = cmd.Title,
                Status = ModuleStatus.Active
            });
        }

        public static Module CreateNew(CreateModule cmd, IValidator<CreateModule> validator)
        {
            validator.ValidateCommand(cmd);

            return new Module(cmd);
        }

        public void UpdateDetails()
        {
            throw new NotImplementedException();
        }

        public void Delete(DeleteModule cmd, IValidator<DeleteModule> validator)
        {
            if (Status == ModuleStatus.Deleted)
                throw new Exception("Module already deleted");

            validator.ValidateCommand(cmd);

            AddEvent(new ModuleDeleted
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == ModuleStatus.Deleted)
                throw new Exception("Module already deleted");

            AddEvent(new ModuleDeleted
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }

        private void Apply(ModuleCreated @event)
        {
            SiteId = @event.SiteId;
            ModuleTypeId = @event.ModuleTypeId;
            Title = @event.Title;
            Status = @event.Status;
        }

        private void Apply(ModuleDeleted @event)
        {
            Status = ModuleStatus.Deleted;
        }
    }
}
