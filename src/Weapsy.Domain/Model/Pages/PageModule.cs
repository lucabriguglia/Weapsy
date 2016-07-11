using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Pages.Commands;

namespace Weapsy.Domain.Model.Pages
{
    public class PageModule : Entity
    {
        public Guid PageId { get; private set; }
        public Guid ModuleId { get; private set; }
        public string Title { get; private set; }
        public string Zone { get; private set; }
        //public Guid ModuleTemplateId { get; private set; }
        public int SortOrder { get; private set; }
        public PageModuleStatus Status { get; private set; }
        public ICollection<PageModuleLocalisation> PageModuleLocalisations { get; private set; }
        public ICollection<Permission> Permissions { get; private set; }

        public PageModule()
        {
        }

        public PageModule(Guid pageId, Guid id, Guid moduleId, string title, string zone, int sortOrder) : base(id)
        {
            PageId = pageId;
            ModuleId = moduleId;
            Title = title;
            Zone = zone;
            SortOrder = sortOrder;
            Status = PageModuleStatus.Active;
        }

        public void UpdateDetails(UpdatePageModuleDetails cmd)
        {
            Title = cmd.Title;

            PageModuleLocalisations.Clear();

            foreach (var localisation in cmd.PageModuleLocalisations)
            {
                AddLocalisation(localisation.LanguageId, localisation.Title);
            }
        }

        private void AddLocalisation(Guid languageId, string title)
        {
            if (PageModuleLocalisations.FirstOrDefault(x => x.LanguageId == languageId) != null)
                throw new Exception("Language already added.");

            PageModuleLocalisations.Add(new PageModuleLocalisation
            {
                PageModuleId = Id,
                LanguageId = languageId,
                Title = title
            });
        }

        public void Reorder(string zone, int sortOrder)
        {
            Zone = zone;
            SortOrder = sortOrder;
        }

        public void Delete()
        {
            if (Status == PageModuleStatus.Deleted)
                throw new Exception("Page module already deleted.");

            Status = PageModuleStatus.Deleted;
        }
    }
}