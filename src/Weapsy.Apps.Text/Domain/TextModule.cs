using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain
{
    public class TextModule : AggregateRoot
    {
        public Guid ModuleId { get; private set; }
        public TextModuleStatus Status { get; private set; }

        public IList<TextVersion> TextVersions { get; private set; } = new List<TextVersion>();

        public TextModule() {}

        private TextModule(CreateTextModule cmd) : base(cmd.Id)
        {
            AddEvent(new TextModuleCreated
            {
                SiteId = cmd.SiteId,
                AggregateRootId = Id,
                ModuleId = cmd.ModuleId,
                VersionId = Guid.NewGuid(),
                Content = cmd.Content,
                Status = TextModuleStatus.Active,
                VersionStatus = TextVersionStatus.Published
            });
        }

        public static TextModule CreateNew(CreateTextModule cmd, IValidator<CreateTextModule> validator)
        {
            validator.ValidateCommand(cmd);

            return new TextModule(cmd);
        }

        public void AddVersion(AddVersion cmd, IValidator<AddVersion> validator)
        {
            if (cmd.Status != TextVersionStatus.Published && cmd.Status != TextVersionStatus.Draft)
                throw new Exception("Status of a new version should be either Published or Draft.");

            validator.ValidateCommand(cmd);

            AddEvent(new VersionAdded
            {
                SiteId = cmd.SiteId,
                ModuleId = cmd.ModuleId,
                AggregateRootId = Id,
                VersionId = cmd.VersionId,
                Content = cmd.Content,
                Description = cmd.Description,
                Status = cmd.Status,
                VersionLocalisations = cmd.VersionLocalisations
            });
        }

        public void PublishVersion()
        {
            throw new NotImplementedException();
        }

        public void DeleteVersion()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        private void Apply(TextModuleCreated @event)
        {
            Id = @event.AggregateRootId;
            ModuleId = @event.ModuleId;
            Status = @event.Status;

            TextVersions.Add(new TextVersion(@event.VersionId,
                @event.AggregateRootId,
                @event.Content,
                string.Empty,
                @event.VersionStatus,
                new List<TextLocalisation>()));
        }

        private void Apply(VersionAdded @event)
        {
            var newVersion = new TextVersion(@event.VersionId,
                Id,
                @event.Content,
                @event.Description,
                @event.Status,
                GetVersionLocalisations(@event));

            TextVersions.Add(newVersion);

            if (newVersion.Status == TextVersionStatus.Published)
            {
                var currentPublishedVersion = TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);
                currentPublishedVersion?.UnPublish();
            }
        }

        private IList<TextLocalisation> GetVersionLocalisations(VersionAdded cmd)
        {
            var localisations = new List<TextLocalisation>();

            foreach (var localisation in cmd.VersionLocalisations)
            {
                localisations.Add(new TextLocalisation
                {
                    TextVersionId = cmd.VersionId,
                    LanguageId = localisation.LanguageId,
                    Content = localisation.Content
                });
            }

            return localisations;
        }
    }
}
