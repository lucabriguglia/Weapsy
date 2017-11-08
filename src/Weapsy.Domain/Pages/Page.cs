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

        private Page(CreatePageCommand cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Status = PageStatus.Hidden;

            SetPageDetails(cmd);

            AddEvent(new PageCreatedEvent
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

        public static Page CreateNew(CreatePageCommand cmd, IValidator<CreatePageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new Page(cmd);
        }

        public void UpdateDetails(UpdatePageDetailsCommand cmd, IValidator<UpdatePageDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            SetPageDetails(cmd);

            AddEvent(new PageDetailsUpdatedEvent
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

        private void SetPageDetails(PageDetailsCommand cmd)
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

        public void AddModule(AddPageModuleCommand cmd, IValidator<AddPageModuleCommand> validator)
        {
            validator.ValidateCommand(cmd);

            AddModule(new PageModule(cmd.PageId, cmd.PageModuleId, cmd.ModuleId, cmd.Title, cmd.Zone, cmd.SortOrder, cmd.PageModulePermissions));
        }

        public void AddModule(PageModule pageModule)
        {
            var alreadyAddedPageModule = PageModules.FirstOrDefault(x => x.ModuleId == pageModule.ModuleId);
            if (alreadyAddedPageModule != null && alreadyAddedPageModule.Status != PageModuleStatus.Deleted)
                throw new Exception("Module already added.");

            var reorderedModules = new List<PageModuleAddedEvent.ReorderedModule>();

            foreach (var existingPageModule in PageModules.Where(x => x.Zone == pageModule.Zone && x.SortOrder >= pageModule.SortOrder))
            {
                existingPageModule.Reorder(existingPageModule.Zone, existingPageModule.SortOrder + 1);
                reorderedModules.Add(new PageModuleAddedEvent.ReorderedModule
                {
                    ModuleId = existingPageModule.ModuleId,
                    SortOrder = existingPageModule.SortOrder
                });
            }

            PageModules.Add(pageModule);

            AddEvent(new PageModuleAddedEvent
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

        public void ReorderPageModules(ReorderPageModulesCommand cmd, IValidator<ReorderPageModulesCommand> validator)
        {
            validator.ValidateCommand(cmd);

            var reorderedPageModules = new List<PageModulesReorderedEvent.PageModule>();

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

                    reorderedPageModules.Add(new PageModulesReorderedEvent.PageModule
                    {
                        ModuleId = pageModule.ModuleId,
                        Zone = zoneName,
                        SortOrder = sortOrder
                    });
                }
            }

            AddEvent(new PageModulesReorderedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModules = reorderedPageModules
            });
        }

        public void UpdateModule(UpdatePageModuleDetailsCommand cmd, IValidator<UpdatePageModuleDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == cmd.ModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.UpdateDetails(cmd);

            AddEvent(new PageModuleDetailsUpdatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModule = pageModule
            });
        }

        public void RemoveModule(RemovePageModuleCommand cmd, IValidator<RemovePageModuleCommand> validator)
        {
            validator.ValidateCommand(cmd);

            var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == cmd.ModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.Delete();

            AddEvent(new PageModuleRemovedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                ModuleId = cmd.ModuleId,
                PageModuleId = pageModule.Id
            });
        }

        public void SetPagePermissions(SetPagePermissionsCommand cmd)
        {
            SetPermissions(cmd.PagePermissions);

            AddEvent(new PagePermissionsSetEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PagePermissions = cmd.PagePermissions
            });
        }

        public void SetModulePermissions(SetPageModulePermissionsCommand cmd)
        {
            var pageModule = PageModules.FirstOrDefault(x => x.Id == cmd.PageModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

            pageModule.SetPermissions(cmd.PageModulePermissions);

            AddEvent(new PageModulePermissionsSetEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                PageModuleId = cmd.PageModuleId,
                PageModulePermissions = cmd.PageModulePermissions
            });
        }

        public void Activate(ActivatePageCommand cmd, IValidator<ActivatePageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Active)
                throw new Exception("Page already active.");

            Status = PageStatus.Active;

            AddEvent(new PageActivatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Hide(HidePageCommand cmd, IValidator<HidePageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Hidden)
                throw new Exception("Page already hidden.");

            if (Status == PageStatus.Deleted)
                throw new Exception("Page is deleted.");

            Status = PageStatus.Hidden;

            AddEvent(new PageHiddenEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        public void Delete(DeletePageCommand cmd, IValidator<DeletePageCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == PageStatus.Deleted)
                throw new Exception("Page already deleted.");

            Status = PageStatus.Deleted;

            AddEvent(new PageDeletedEvent
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