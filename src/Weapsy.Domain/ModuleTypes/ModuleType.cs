using System;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Domain.ModuleTypes
{
    public class ModuleType : AggregateRoot
    {
        public Guid AppId { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ViewType ViewType { get; private set; }
        public string ViewName { get; private set; }
        public EditType EditType { get; private set; }
        public string EditUrl { get; private set; }
        public ModuleTypeStatus Status { get; private set; }

        public ModuleType(){}

        private ModuleType(CreateModuleTypeCommand cmd) : base(cmd.Id)
        {
            AppId = cmd.AppId;
            Status = ModuleTypeStatus.Active;
            UpdateDetails(cmd);

            AddEvent(new ModuleTypeCreatedEvent
            {
                AppId = AppId,
                AggregateRootId = Id,
                Name = Name,
                Title = Title,
                Description = Description,
                ViewType = ViewType,
                ViewName = ViewName,
                EditType = EditType,
                EditUrl = EditUrl,
                Status = Status
            });
        }

        public static ModuleType CreateNew(CreateModuleTypeCommand cmd, IValidator<CreateModuleTypeCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new ModuleType(cmd);
        }

        public void UpdateDetails(UpdateModuleTypeDetailsCommand cmd, IValidator<UpdateModuleTypeDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            UpdateDetails(cmd);

            AddEvent(new ModuleTypeDetailsUpdatedEvent
            {
                AggregateRootId = Id,
                Name = Name,
                Title = Title,
                Description = Description,
                ViewType = ViewType,
                ViewName = ViewName,
                EditType = EditType,
                EditUrl = EditUrl,
            });
        }

        private void UpdateDetails(ModuleTypeDetailsCommand cmd)
        {
            Name = cmd.Name;
            Title = cmd.Title;
            Description = cmd.Description;
            ViewType = cmd.ViewType;
            ViewName = cmd.ViewName;
            EditType = cmd.EditType;
            EditUrl = cmd.EditUrl;
        }

        public void Delete(DeleteModuleTypeCommand cmd, IValidator<DeleteModuleTypeCommand> validator)
        {
            if (Status == ModuleTypeStatus.Deleted)
                throw new Exception("Module type already deleted.");

            validator.ValidateCommand(cmd);

            Status = ModuleTypeStatus.Deleted;

            AddEvent(new ModuleTypeDeletedEvent
            {
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}
