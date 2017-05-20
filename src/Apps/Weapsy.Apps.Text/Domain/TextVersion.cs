using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain
{
    public class TextVersion : Entity
    {
        public Guid TextModuleId { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public TextVersionStatus Status { get; private set; }

        public IList<TextLocalisation> TextLocalisations { get; private set; } = new List<TextLocalisation>();

        public TextVersion() {}

        public TextVersion(Guid id, 
            Guid textModuleId, 
            string content, 
            string description, 
            TextVersionStatus status,
            IList<TextLocalisation> localisations)
        {
            Id = id;
            TextModuleId = textModuleId;
            Content = content;
            Description = description;
            Status = status;
            TextLocalisations = localisations;
        }

        private void AddLocalisation(Guid languageId, string content)
        {
            if (TextLocalisations.FirstOrDefault(x => x.LanguageId == languageId) != null)
                throw new Exception("Language already added.");

            TextLocalisations.Add(new TextLocalisation
            {
                TextVersionId = Id,
                LanguageId = languageId,
                Content = content
            });
        }

        public void UnPublish()
        {
            Status = TextVersionStatus.PreviouslyPublished;
        }
    }
}
