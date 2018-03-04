using FluentValidation;
using System;
using Weapsy.Cqrs.Domain;
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

        private Language(CreateLanguage cmd, ILanguageSortOrderGenerator languageSortOrderGenerator)
            : base(cmd.Id)
        {
            AddEvent(new LanguageCreated
            {
                SiteId = cmd.SiteId,
                AggregateRootId = Id,
                Name = cmd.Name,
                CultureName = cmd.CultureName,
                Url = cmd.Url,
                SortOrder = languageSortOrderGenerator.GenerateNextSortOrder(cmd.SiteId),
                Status = LanguageStatus.Hidden
            });
        }

        public static Language CreateNew(CreateLanguage cmd,
            IValidator<CreateLanguage> validator,
            ILanguageSortOrderGenerator sortOrderGenerator)
        {
            validator.ValidateCommand(cmd);

            return new Language(cmd, sortOrderGenerator);
        }

        public void UpdateDetails(UpdateLanguageDetails cmd, IValidator<UpdateLanguageDetails> validator)
        {
            validator.ValidateCommand(cmd);

            AddEvent(new LanguageDetailsUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = cmd.Name,
                CultureName = cmd.CultureName,
                Url = cmd.Url
            });
        }

        public void Reorder(int sortOrder)
        {
            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language is deleted and cannot be reordered.");

            AddEvent(new LanguageReordered
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                SortOrder = sortOrder
            });
        }

        public void Activate(ActivateLanguage cmd, IValidator<ActivateLanguage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Active)
                throw new Exception("Language already active.");

            AddEvent(new LanguageActivated
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Hide(HideLanguage cmd, IValidator<HideLanguage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Hidden)
                throw new Exception("Language already hidden.");

            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language is deleted.");

            AddEvent(new LanguageHidden
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete(DeleteLanguage cmd, IValidator<DeleteLanguage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == LanguageStatus.Deleted)
                throw new Exception("Language already deleted.");

            AddEvent(new LanguageDeleted
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }

        private void Apply(LanguageCreated @event)
        {
            Id = @event.AggregateRootId;
            SiteId = @event.SiteId;
            Name = @event.Name;
            CultureName = @event.CultureName;
            Url = @event.Url;
            SortOrder = @event.SortOrder;
            Status = @event.Status;
        }

        private void Apply(LanguageDetailsUpdated @event)
        {
            Name = @event.Name;
            CultureName = @event.CultureName;
            Url = @event.Url;
        }

        private void Apply(LanguageReordered @event)
        {
            SortOrder = @event.SortOrder;
        }

        private void Apply(LanguageActivated @event)
        {
            Status = LanguageStatus.Active;
        }

        private void Apply(LanguageHidden @event)
        {
            Status = LanguageStatus.Hidden;
        }

        private void Apply(LanguageDeleted @event)
        {
            Status = LanguageStatus.Deleted;
        }
    }
}
