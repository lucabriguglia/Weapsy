using FluentValidation;
using System;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Events;

namespace Weapsy.Domain.Languages
{
    public class Language : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Name { get; private set; }
        public string CultureName { get; private set; }
        public string Url { get; private set; }
        public string Flag { get; private set; }
        public int SortOrder { get; private set; }
        public LanguageStatus Status { get; private set; }

        public Language(){}

        private Language(CreateLanguageCommand cmd, ILanguageSortOrderGenerator languageSortOrderGenerator)
            : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Name = cmd.Name;
            CultureName = cmd.CultureName;
            Url = cmd.Url;
            SortOrder = languageSortOrderGenerator.GenerateNextSortOrder(cmd.SiteId);
            Status = LanguageStatus.Hidden;

            AddEvent(new LanguageCreatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                CultureName = CultureName,
                Url = Url,
                SortOrder = SortOrder,
                Status = Status
            });
        }

        public static Language CreateNew(CreateLanguageCommand cmd,
            IValidator<CreateLanguageCommand> validator,
            ILanguageSortOrderGenerator sortOrderGenerator)
        {
            validator.ValidateCommand(cmd);

            return new Language(cmd, sortOrderGenerator);
        }

        public void UpdateDetails(UpdateLanguageDetailsCommand cmd, IValidator<UpdateLanguageDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            Name = cmd.Name;
            CultureName = cmd.CultureName;
            Url = cmd.Url;

            AddEvent(new LanguageDetailsUpdatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                CultureName = CultureName,
                Url = Url
            });
        }

        public void Reorder(int sortOrder)
        {
            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language is deleted and cannot be reordered.");

            SortOrder = sortOrder;

            AddEvent(new LanguageReorderedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                SortOrder = sortOrder
            });
        }

        public void Activate(ActivateLanguageCommand cmd, IValidator<ActivateLanguageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Active)
                throw new Exception("Language already active.");

            Status = LanguageStatus.Active;

            AddEvent(new LanguageActivatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Hide(HideLanguageCommand cmd, IValidator<HideLanguageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Hidden)
                throw new Exception("Language already hidden.");

            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language is deleted.");

            Status = LanguageStatus.Hidden;

            AddEvent(new LanguageHiddenEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete(DeleteLanguageCommand cmd, IValidator<DeleteLanguageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language already deleted.");

            Status = LanguageStatus.Deleted;

            AddEvent(new LanguageDeletedEvent
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
