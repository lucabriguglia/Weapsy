using System;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;
using System.Collections.Generic;
using System.Linq;

namespace Weapsy.Domain.Pages
{
    public class Page : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Name { get; private set; }
        public PageStatus Status { get; private set; }
        public string Url { get; private set; }
        public string Title { get; private set; }
        public string MetaDescription { get; private set; }
        public string MetaKeywords { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public Guid? ThemeId { get; private set; }
        public Guid? PageTemplateId { get; private set; }
        public Guid? ModuleTemplateId { get; private set; }
        public ICollection<PageLocalisation> PageLocalisations { get; private set; } = new List<PageLocalisation>();
        public ICollection<PageModule> PageModules { get; private set; } = new List<PageModule>();
        public ICollection<PagePermission> PagePermissions { get; private set; } = new List<PagePermission>();

        public Page() {}

        private Page(CreatePage cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Status = PageStatus.Hidden;

            SetPageDetails(cmd);

            AddEvent(new PageCreated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                Url = Url,
                Title = Title,
                MetaDescription = MetaDescription,
                MetaKeywords = MetaKeywords,
                PageLocalisations = PageLocalisations,
                PagePermissions = PagePermissions,
                Status = Status,
                MenuIds = cmd.MenuIds
            });
        }

        public static Page CreateNew(CreatePage cmd, IValidator<CreatePage> validator)
        {
            validator.ValidateCommand(cmd);

            return new Page(cmd);
        }

        public void UpdateDetails(UpdatePageDetails cmd, IValidator<UpdatePageDetails> validator)
        {
            validator.ValidateCommand(cmd);

            SetPageDetails(cmd);

            AddEvent(new PageDetailsUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                Url = Url,
                Title = Title,
                MetaDescription = MetaDescription,
                MetaKeywords = MetaKeywords,
                PageLocalisations = PageLocalisations,
                PagePermissions = PagePermissions
            });
        }

        private void SetPageDetails(PageDetails cmd)
        {
            Name = cmd.Name;
            Url = cmd.Url;
            Title = cmd.Title;
            MetaDescription = cmd.MetaDescription;
            MetaKeywords = cmd.MetaKeywords;

            SetLocalisations(cmd.PageLocalisations);
            SetPermissions(cmd.PagePermissions);
        }

        private void SetLocalisations(IEnumerable<PageLocalisation> pageLocalisations)
        {
            PageLocalisations.Clear();

            foreach (var localisation in pageLocalisations)
            {
                AddLocalisation(new PageLocalisation
                {
                    PageId = Id,
                    LanguageId = localisation.LanguageId,
                    Url = localisation.Url,
                    Title = localisation.Title,
                    MetaDescription = localisation.MetaDescription,
                    MetaKeywords = localisation.MetaKeywords
                });
            }
        }

        private void SetPermissions(IEnumerable<PagePermission> pagePermissions)
        {
            PagePermissions.Clear();

            foreach (var permission in pagePermissions)
            {
                if (PagePermissions.FirstOrDefault(x => x.RoleId == permission.RoleId && x.Type == permission.Type) == null)
                {
                    PagePermissions.Add(new PagePermission
                    {
                        PageId = Id,
                        RoleId = permission.RoleId,
                        Type = permission.Type
                    });
                }
            }
        }

        private void AddLocalisation(PageLocalisation localisation)
        {
            if (PageLocalisations.FirstOrDefault(x => x.LanguageId == localisation.LanguageId) != null)
                throw new Exception("Language already added.");

            PageLocalisations.Add(localisation);
        }

        public void AddModule(AddPageModule cmd, IValidator<AddPageModule> validator)
        {
            validator.ValidateCommand(cmd);

            AddModule(new PageModule(cmd.PageId, cmd.PageModuleId, cmd.ModuleId, cmd.Title, cmd.Zone, cmd.SortOrder, cmd.PageModulePermissions));
        }

        public void AddModule(PageModule pageModule)
        {
            var alreadyAddedPageModule = PageModules.FirstOrDefault(x => x.ModuleId == pageModule.ModuleId);
            if (alreadyAddedPageModule != null && alreadyAddedPageModule.Status != PageModuleStatus.Deleted)
                throw new Exception("Module already added.");

            var reorderedModules = new List<PageModuleAdded.ReorderedModule>();

            foreach (var existingPageModule in PageModules.Where(x => x.Zone == pageModule.Zone && x.SortOrder >= pageModule.SortOrder))
            {
                existingPageModule.Reorder(existingPageModule.Zone, existingPageModule.SortOrder + 1);
                reorderedModules.Add(new PageModuleAdded.ReorderedModule
                {
                    ModuleId = existingPageModule.ModuleId,
                    SortOrder = existingPageModule.SortOrder
                });
            }

            PageModules.Add(pageModule);

            AddEvent(new PageModuleAdded
            {
                SiteId = SiteId,
                PageModuleId = pageModule.Id,
                ModuleId = pageModule.ModuleId,
                AggregateRootId = Id,
                Title = pageModule.Title,
                Zone = pageModule.Zone,
                SortOrder = pageModule.SortOrder,
                PageModuleStatus = PageModuleStatus.Active,
                ReorderedModules = reorderedModules
            });
        }

        public void ReorderPageModules(ReorderPageModules cmd, IValidator<ReorderPageModules> validator)
        {
            validator.ValidateCommand(cmd);

            var reorderedPageModules = new List<PageModulesReordered.PageModule>();

            foreach (var zone in cmd.Zones)
            {
                for (int i = 0; i < zone.Modules.Count; i++)
                {
                    var moduleId = zone.Modules[i];
                    var zoneName = zone.Name;
                    var sortOrder = i + 1;

                    var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == moduleId);

                    if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                        throw new Exception("Page module not found.");

                    if (pageModule.Zone == zoneName && pageModule.SortOrder == sortOrder)
                        continue;

                    pageModule.Reorder(zoneName, sortOrder);

                    reorderedPageModules.Add(new PageModulesReordered.PageModule
                    {
                        ModuleId = pageModule.ModuleId,
                        Zone = zoneName,
                        SortOrder = sortOrder
                    });
                }
            }

            AddEvent(new PageModulesReordered
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModules = reorderedPageModules
            });
        }

        public void UpdateModule(UpdatePageModuleDetails cmd, IValidator<UpdatePageModuleDetails> validator)
        {
            validator.ValidateCommand(cmd);

            var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == cmd.ModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.UpdateDetails(cmd);

            AddEvent(new PageModuleDetailsUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModule = pageModule
            });
        }

        public void RemoveModule(RemovePageModule cmd, IValidator<RemovePageModule> validator)
        {
            validator.ValidateCommand(cmd);

            var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == cmd.ModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.Delete();

            AddEvent(new PageModuleRemoved
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                ModuleId = cmd.ModuleId,
                PageModuleId = pageModule.Id
            });
        }

        public void SetPagePermissions(SetPagePermissions cmd)
        {
            SetPermissions(cmd.PagePermissions);

            AddEvent(new PagePermissionsSet
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PagePermissions = cmd.PagePermissions
            });
        }

        public void SetModulePermissions(SetPageModulePermissions cmd)
        {
            var pageModule = PageModules.FirstOrDefault(x => x.Id == cmd.PageModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.SetPermissions(cmd.PageModulePermissions);

            AddEvent(new PageModulePermissionsSet
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModuleId = cmd.PageModuleId,
                PageModulePermissions = cmd.PageModulePermissions
            });
        }

        public void Activate(ActivatePage cmd, IValidator<ActivatePage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Active)
                throw new Exception("Page already active.");

            Status = PageStatus.Active;

            AddEvent(new PageActivated
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Hide(HidePage cmd, IValidator<HidePage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Hidden)
                throw new Exception("Page already hidden.");

            if (Status == PageStatus.Deleted)
                throw new Exception("Page is deleted.");

            Status = PageStatus.Hidden;

            AddEvent(new PageHidden
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete(DeletePage cmd, IValidator<DeletePage> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Deleted)
                throw new Exception("Page already deleted.");

            Status = PageStatus.Deleted;

            AddEvent(new PageDeleted
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