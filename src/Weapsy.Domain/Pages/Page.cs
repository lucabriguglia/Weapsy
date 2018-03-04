using System;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Cqrs.Domain;

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
            AddEvent(new PageCreated
            {
                AggregateRootId = Id,
                SiteId = cmd.SiteId,               
                Name = cmd.Name,
                Url = cmd.Url,
                Title = cmd.Title,
                MetaDescription = cmd.MetaDescription,
                MetaKeywords = cmd.MetaKeywords,
                PageLocalisations = cmd.PageLocalisations,
                PagePermissions = cmd.PagePermissions,
                MenuIds = cmd.MenuIds,
                Status = PageStatus.Hidden
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

            AddEvent(new PageDetailsUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = cmd.Name,
                Url = cmd.Url,
                Title = cmd.Title,
                MetaDescription = cmd.MetaDescription,
                MetaKeywords = cmd.MetaKeywords,
                PageLocalisations = cmd.PageLocalisations,
                PagePermissions = cmd.PagePermissions
            });
        }

        public void AddModule(AddPageModule cmd, IValidator<AddPageModule> validator)
        {
            validator.ValidateCommand(cmd);

            AddModule(new PageModule(cmd.PageId, cmd.PageModuleId, cmd.ModuleId, cmd.Title, cmd.Zone, cmd.SortOrder, cmd.PageModulePermissions));
        }

        public void AddModule(PageModule pageModule)
        {
            var alreadyAddedPageModule = PageModules.FirstOrDefault(x => x.Id == pageModule.Id);
            if (alreadyAddedPageModule != null && alreadyAddedPageModule.Status != PageModuleStatus.Deleted)
                throw new Exception("Module already added.");

            var reorderedModules = new List<PageModuleAdded.ReorderedModule>();

            foreach (var existingPageModule in PageModules.Where(x => x.Zone == pageModule.Zone && x.SortOrder >= pageModule.SortOrder))
            {
                reorderedModules.Add(new PageModuleAdded.ReorderedModule
                {
                    PageModuleId = existingPageModule.Id,
                    Zone = existingPageModule.Zone,
                    SortOrder = existingPageModule.SortOrder + 1
                });
            }

            AddEvent(new PageModuleAdded
            {
                AggregateRootId = Id,
                SiteId = SiteId,
                PageModule = pageModule,
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

                    reorderedPageModules.Add(new PageModulesReordered.PageModule
                    {
                        PageModuleId = pageModule.Id,
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

            AddEvent(new PageModuleDetailsUpdated
            {
                AggregateRootId = Id,
                SiteId = SiteId,
                PageId = cmd.PageId,
                PageModuleId = pageModule.Id,
                Title = cmd.Title,
                InheritPermissions = cmd.InheritPermissions,
                PageModuleLocalisations = cmd.PageModuleLocalisations,
                PageModulePermissions = cmd.PageModulePermissions
            });
        }

        public void RemoveModule(RemovePageModule cmd, IValidator<RemovePageModule> validator)
        {
            validator.ValidateCommand(cmd);

            var pageModule = PageModules.FirstOrDefault(x => x.ModuleId == cmd.ModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                throw new Exception("Page module not found.");

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

        private void Apply(PageActivated @event)
        {
            Status = PageStatus.Active;
        }

        private void Apply(PageCreated @event)
        {
            Id = @event.AggregateRootId;
            SiteId = @event.SiteId;
            Status = @event.Status;

            UpdatePageDetails(@event);
        }

        private void Apply(PageDeleted @event)
        {
            Status = PageStatus.Deleted;
        }

        private void Apply(PageDetailsUpdated @event)
        {
            UpdatePageDetails(@event);
        }

        private void Apply(PageHidden @event)
        {
            Status = PageStatus.Hidden;
        }

        private void Apply(PageModuleAdded @event)
        {
            PageModules.Add(@event.PageModule);

            foreach (var reorderedModule in @event.ReorderedModules)
            {
                var pageModule = PageModules.FirstOrDefault(x => x.Id == reorderedModule.PageModuleId);
                pageModule?.Reorder(reorderedModule.Zone, reorderedModule.SortOrder);
            }
        }

        private void Apply(PageModuleDetailsUpdated @event)
        {
            var pageModule = PageModules.FirstOrDefault(x => x.Id == @event.PageModuleId);
            pageModule?.UpdateDetails(@event);
        }

        private void Apply(PageModulePermissionsSet @event)
        {
            var pageModule = PageModules.FirstOrDefault(x => x.Id == @event.PageModuleId);
            pageModule?.SetPermissions(@event.PageModulePermissions);
        }

        private void Apply(PageModuleRemoved @event)
        {
            var pageModule = PageModules.FirstOrDefault(x => x.Id == @event.PageModuleId);
            pageModule?.Delete();
        }

        private void Apply(PageModulesReordered @event)
        {
            foreach (var reorderedPageModule in @event.PageModules)
            {
                var pageModule = PageModules.FirstOrDefault(x => x.Id == reorderedPageModule.PageModuleId);
                pageModule?.Reorder(reorderedPageModule.Zone, reorderedPageModule.SortOrder);
            }
        }

        private void Apply(PagePermissionsSet @event)
        {
            SetPermissions(@event.PagePermissions);
        }

        private void UpdatePageDetails(PageDetailsBase @event)
        {
            Name = @event.Name;
            Url = @event.Url;
            Title = @event.Title;
            MetaDescription = @event.MetaDescription;
            MetaKeywords = @event.MetaKeywords;

            SetLocalisations(@event.PageLocalisations);
            SetPermissions(@event.PagePermissions);
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

        private void AddLocalisation(PageLocalisation localisation)
        {
            if (PageLocalisations.FirstOrDefault(x => x.LanguageId == localisation.LanguageId) == null)
                PageLocalisations.Add(localisation);
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
    }
}