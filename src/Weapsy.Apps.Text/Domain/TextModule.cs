using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Events;
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
            ModuleId = cmd.ModuleId;
            Status = TextModuleStatus.Active;

            var versionId = Guid.NewGuid();

            TextVersions.Add(new TextVersion(versionId, 
                Id, 
                cmd.Content, 
                string.Empty, 
                TextVersionStatus.Published,
                new List<TextLocalisation>()));

            AddEvent(new TextModuleCreated
            {
                SiteId = cmd.SiteId,
                AggregateRootId = Id,
                ModuleId = ModuleId,
                VersionId = versionId,
                Content = cmd.Content
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

            var newVersion = new TextVersion(cmd.VersionId,
                Id,
                cmd.Content,
                cmd.Description,
                cmd.Status,
                GetVersionLocalisations(cmd));

            TextVersions.Add(newVersion);

            if (newVersion.Status == TextVersionStatus.Published)
            {
                var currentPublishedVersion = TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);
                currentPublishedVersion?.UnPublish();
            }

            AddEvent(new VersionAdded
            {
                SiteId = cmd.SiteId,
                ModuleId = cmd.ModuleId,
                AggregateRootId = Id,
                VersionId = cmd.VersionId,
                Content = cmd.Content,
                Description = cmd.Description,
                Status = cmd.Status
            });
        }

        private IList<TextLocalisation> GetVersionLocalisations(AddVersion cmd)
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
    }
}
