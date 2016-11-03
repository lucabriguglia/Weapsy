using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages
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
        public bool InheritPermissions { get; private set; }
        public ICollection<PageModuleLocalisation> PageModuleLocalisations { get; private set; } = new List<PageModuleLocalisation>();
        public ICollection<PageModulePermission> PageModulePermissions { get; private set; } = new List<PageModulePermission>();

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
            InheritPermissions = true;
        }

        public void UpdateDetails(UpdatePageModuleDetails cmd)
        {
            Title = cmd.Title;
            InheritPermissions = cmd.InheritPermissions;

            SetLocalisations(cmd.PageModuleLocalisations);
            SetPermissions(cmd.PageModulePermissions);
        }

        private void SetLocalisations(List<PageModuleLocalisation> pageModuleLocalisations)
        {
            PageModuleLocalisations.Clear();

            foreach (var localisation in pageModuleLocalisations)
                AddLocalisation(localisation.LanguageId, localisation.Title);
        }

        public void SetPermissions(IList<PageModulePermission> permissions)
        {
            PageModulePermissions.Clear();

            foreach (var permission in permissions)
            {
                if (PageModulePermissions.FirstOrDefault(x => x.RoleId == permission.RoleId && x.Type == permission.Type) == null)
                {
                    PageModulePermissions.Add(new PageModulePermission
                    {
                        PageModuleId = Id,
                        RoleId = permission.RoleId,
                        Type = permission.Type
                    });
                }
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