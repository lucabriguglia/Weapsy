using FluentValidation;
using System;
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

        private Template(CreateTemplateCommand cmd)
            : base(cmd.Id)
        {
            Name = cmd.Name;
            Description = cmd.Description;
            ViewName = cmd.ViewName;
            Status = TemplateStatus.Hidden;
            Type = cmd.Type;

            AddEvent(new TemplateCreatedEvent
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                ViewName = ViewName,
                Status = Status,
                Type = Type
            });
        }

        public static Template CreateNew(CreateTemplateCommand cmd, IValidator<CreateTemplateCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new Template(cmd);
        }

        public void UpdateDetails(UpdateTemplateDetailsCommand cmd, IValidator<UpdateTemplateDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            Name = cmd.Name;
            Description = cmd.Description;
            ViewName = cmd.ViewName;
            Type = cmd.Type;

            AddEvent(new TemplateDetailsUpdatedEvent
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                ViewName = ViewName,
                Type = Type
            });
        }

        public void Activate()
        {
            if (Status == TemplateStatus.Active)
                throw new Exception("Template already active.");

            Status = TemplateStatus.Active;

            AddEvent(new TemplateActivatedEvent
            {
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == TemplateStatus.Deleted)
                throw new Exception("Template already deleted.");

            Status = TemplateStatus.Deleted;

            AddEvent(new TemplateDeletedEvent
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
