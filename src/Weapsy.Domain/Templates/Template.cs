using FluentValidation;
using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Events;

namespace Weapsy.Domain.Templates
{
    public class Template : AggregateRoot
    {
        public Guid ThemeId { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ViewName { get; private set; }
        public int SortOrder { get; private set; }
        public TemplateStatus Status { get; private set; }
        public TemplateType Type { get; private set; }

        public Template() { }

        private Template(CreateTemplate cmd) : base(cmd.Id)
        {
            AddEvent(new TemplateCreated
            {
                AggregateRootId = Id,
                Name = cmd.Name,
                Description = cmd.Description,
                ViewName = cmd.ViewName,
                Status = TemplateStatus.Hidden,
                Type = cmd.Type
            });
        }

        public static Template CreateNew(CreateTemplate cmd, IValidator<CreateTemplate> validator)
        {
            validator.ValidateCommand(cmd);

            return new Template(cmd);
        }

        public void UpdateDetails(UpdateTemplateDetails cmd, IValidator<UpdateTemplateDetails> validator)
        {
            validator.ValidateCommand(cmd);

            AddEvent(new TemplateDetailsUpdated
            {
                AggregateRootId = Id,
                Name = cmd.Name,
                Description = cmd.Description,
                ViewName = cmd.ViewName,
                Type = cmd.Type
            });
        }

        public void Activate()
        {
            if (Status == TemplateStatus.Active)
                throw new Exception("Template already active.");

            AddEvent(new TemplateActivated
            {
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == TemplateStatus.Deleted)
                throw new Exception("Template already deleted.");

            AddEvent(new TemplateDeleted
            {
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }

        private void Apply(TemplateActivated @event)
        {
            Status = TemplateStatus.Active;
        }

        private void Apply(TemplateCreated @event)
        {
            Name = @event.Name;
            Description = @event.Description;
            ViewName = @event.ViewName;
            Status = @event.Status;
            Type = @event.Type;
        }

        private void Apply(TemplateDeleted @event)
        {
            Status = TemplateStatus.Deleted;
        }

        private void Apply(TemplateDetailsUpdated @event)
        {
            Name = @event.Name;
            Description = @event.Description;
            ViewName = @event.ViewName;
            Type = @event.Type;
        }
    }
}
