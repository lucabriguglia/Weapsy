using FluentValidation;
using System;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Events;

namespace Weapsy.Domain.Templates
{
    public class Template : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ViewName { get; private set; }
        public int SortOrder { get; private set; }
        public TemplateStatus Status { get; private set; }
        public TemplateType TemplateType { get; private set; }

        public Template() { }

        private Template(CreateTemplate cmd)
            : base(cmd.Id)
        {
            Name = cmd.Name;
            Description = cmd.Description;
            ViewName = cmd.ViewName;
            Status = TemplateStatus.Hidden;

            AddEvent(new TemplateCreated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                ViewName = ViewName,
                Status = Status
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

            Name = cmd.Name;
            Description = cmd.Description;
            ViewName = cmd.ViewName;

            AddEvent(new TemplateDetailsUpdated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                ViewName = ViewName
            });
        }

        public void Activate()
        {
            if (Status == TemplateStatus.Active)
                throw new Exception("Template already active.");

            Status = TemplateStatus.Active;

            AddEvent(new TemplateActivated
            {
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == TemplateStatus.Deleted)
                throw new Exception("Template already deleted.");

            Status = TemplateStatus.Deleted;

            AddEvent(new TemplateDeleted
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
