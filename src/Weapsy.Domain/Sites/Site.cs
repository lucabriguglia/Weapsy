using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Events;

namespace Weapsy.Domain.Sites
{
    public class Site : AggregateRoot
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Copyright { get; private set; }
        public string Logo { get; private set; }
        public string Title { get; private set; }
        public string MetaDescription { get; private set; }
        public string MetaKeywords { get; private set; }
        public Guid HomePageId { get; private set; }
        public Guid ThemeId { get; private set; }
        public Guid PageTemplateId { get; private set; }
        public Guid ModuleTemplateId { get; private set; }
        public bool AddLanguageSlug { get; private set; }
        public SiteStatus Status { get; set; }
        public ICollection<SiteLocalisation> SiteLocalisations { get; private set; } = new List<SiteLocalisation>();

        public Site()
        {
        }

        private Site(CreateSite cmd) : base(cmd.Id)
        {
            Name = cmd.Name;
            Status = SiteStatus.Active;

            AddEvent(new SiteCreated
            {
                AggregateRootId = Id,
                Name = Name,
                Status = Status
            });
        }

        public static Site CreateNew(CreateSite cmd, IValidator<CreateSite> validator)
        {
            validator.ValidateCommand(cmd);

            return new Site(cmd);
        }

        public void UpdateName()
        {
            throw new NotImplementedException();
        }

        public void UpdateDetails(UpdateSiteDetails cmd, IValidator<UpdateSiteDetails> validator)
        {
            validator.ValidateCommand(cmd);

            SetSiteDetails(cmd);

            AddEvent(new SiteDetailsUpdated
            {
                AggregateRootId = Id,
                Name = Name,
                Url = Url,
                Title = Title,
                MetaDescription = MetaDescription,
                MetaKeywords = MetaKeywords,
                SiteLocalisations = SiteLocalisations,
                AddLanguageSlug = AddLanguageSlug,
                HomePageId = HomePageId,
                ThemeId = ThemeId
            });
        }

        private void SetSiteDetails(UpdateSiteDetails cmd)
        {
            Url = cmd.Url;
            Title = cmd.Title;
            MetaDescription = cmd.MetaDescription;
            MetaKeywords = cmd.MetaKeywords;
            HomePageId = cmd.HomePageId;
            ThemeId = cmd.ThemeId;
            AddLanguageSlug = cmd.AddLanguageSlug;

            SetLocalisations(cmd.SiteLocalisations);
        }

        private void SetLocalisations(IEnumerable<SiteLocalisation> localisations)
        {
            SiteLocalisations.Clear();

            foreach (var localisation in localisations)
            {
                if (SiteLocalisations.FirstOrDefault(x => x.LanguageId == localisation.LanguageId) != null)
                    continue;

                localisation.SiteId = Id;

                SiteLocalisations.Add(localisation);
            }
        }

        public void Close()
        {
            switch (Status)
            {
                case SiteStatus.Closed:
                    throw new Exception("Site already closed.");
                case SiteStatus.Deleted:
                    throw new Exception("Site is deleted.");
            }

            Status = SiteStatus.Closed;

            AddEvent(new SiteClosed
            {
                Name = Name,
                AggregateRootId = Id
            });
        }

        public void Reopen()
        {
            switch (Status)
            {
                case SiteStatus.Active:
                    throw new Exception("Site already active.");
                case SiteStatus.Deleted:
                    throw new Exception("Site is deleted.");
            }

            Status = SiteStatus.Active;

            AddEvent(new SiteReopened
            {
                Name = Name,
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == SiteStatus.Deleted)
                throw new Exception("Site already deleted.");

            Status = SiteStatus.Deleted;

            AddEvent(new SiteDeleted
            {
                Name = Name,
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}
