using System;
using FluentValidation;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Domain.ModuleTypes
{
    public class ModuleType : AggregateRoot
    {
        public Guid? AppId { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ViewType ViewType { get; private set; }
        public string ViewName { get; private set; }
        public EditType EditType { get; private set; }
        public string EditUrl { get; private set; }
        public ModuleTypeStatus Status { get; private set; }

        public ModuleType(){}

        private ModuleType(CreateModuleType cmd) : base(cmd.Id)
        {
            AddEvent(new ModuleTypeCreated
            {
                AppId = cmd.AppId,
                AggregateRootId = Id,
                Name = cmd.Name,
                Title = cmd.Title,
                Description = cmd.Description,
                ViewType = cmd.ViewType,
                ViewName = cmd.ViewName,
                EditType = cmd.EditType,
                EditUrl = cmd.EditUrl,
                Status = ModuleTypeStatus.Active
            });
        }

        public static ModuleType CreateNew(CreateModuleType cmd, IValidator<CreateModuleType> validator)
        {
            validator.ValidateCommand(cmd);

            return new ModuleType(cmd);
        }

        public void UpdateDetails(UpdateModuleTypeDetails cmd, IValidator<UpdateModuleTypeDetails> validator)
        {
            validator.ValidateCommand(cmd);

            AddEvent(new ModuleTypeDetailsUpdated
            {
                AggregateRootId = Id,
                Name = cmd.Name,
                Title = cmd.Title,
                Description = cmd.Description,
                ViewType = cmd.ViewType,
                ViewName = cmd.ViewName,
                EditType = cmd.EditType,
                EditUrl = cmd.EditUrl
            });
        }

        public void Delete(DeleteModuleType cmd, IValidator<DeleteModuleType> validator)
        {
            if (Status == ModuleTypeStatus.Deleted)
                throw new Exception("Module type already deleted.");

            validator.ValidateCommand(cmd);

            AddEvent(new ModuleTypeDeleted
            {
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }

        private void Apply(ModuleTypeCreated @event)
        {
            Id = @event.AggregateRootId;
            AppId = @event.AppId;
            Status = @event.Status;
            Name = @event.Name;
            Title = @event.Title;
            Description = @event.Description;
            ViewType = @event.ViewType;
            ViewName = @event.ViewName;
            EditType = @event.EditType;
            EditUrl = @event.EditUrl;
        }

        private void Apply(ModuleTypeDetailsUpdated @event)
        {
            Name = @event.Name;
            Title = @event.Title;
            Description = @event.Description;
            ViewType = @event.ViewType;
            ViewName = @event.ViewName;
            EditType = @event.EditType;
            EditUrl = @event.EditUrl;
        }

        private void Apply(ModuleTypeDeleted @event)
        {
            Status = ModuleTypeStatus.Deleted;
        }
    }
}
