using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Pages;
using PageDbEntity = Weapsy.Data.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Data.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Data.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Data.Entities.PageModuleLocalisation;
using PagePermissionDbEntity = Weapsy.Data.Entities.PagePermission;
using PageModulePermissionDbEntity = Weapsy.Data.Entities.PageModulePermission;

namespace Weapsy.Data.Domain
{
    public class PageRepository : IPageRepository
    {
        private readonly IContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public PageRepository(IContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Page GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Pages
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.Id == id && x.Status != PageStatus.Deleted);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Page GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Pages
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.SiteId == siteId && x.Id == id && x.Status != PageStatus.Deleted);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Guid GetPageIdByName(Guid siteId, string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Pages
                    .Where(x => x.SiteId == siteId && x.Name == name && x.Status != PageStatus.Deleted)
                    .Select(x => x.Id)
                    .FirstOrDefault();
            }
        }

        public Guid GetPageIdBySlug(Guid siteId, string slug)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Pages
                    .Where(x => x.SiteId == siteId && x.Url == slug && x.Status != PageStatus.Deleted)
                    .Select(x => x.Id)
                    .FirstOrDefault();
            }
        }

        public Guid GetPageIdByLocalisedSlug(Guid siteId, string slug)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.PageLocalisations
                    .Where(x => x.Page.SiteId == siteId && x.Url == slug && x.Page.Status != PageStatus.Deleted)
                    .Select(x => x.PageId)
                    .FirstOrDefault();
            }
        }

        public void Create(Page page)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<PageDbEntity>(page);
                context.Pages.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Page page)
        {
            using (var context = _dbContextFactory.Create())
            {
                var pageDbEntity = _mapper.Map<PageDbEntity>(page);

                context.Entry(pageDbEntity).State = EntityState.Modified;

                UpdatePageLocalisations(context, pageDbEntity.Id, pageDbEntity.PageLocalisations);
                UpdatePageModules(context, pageDbEntity.PageModules);
                UpdatePagePermissions(context, pageDbEntity.Id, pageDbEntity.PagePermissions);

                context.SaveChanges();
            }
        }

        private void UpdatePageLocalisations(WeapsyDbContext context, Guid pageId, IEnumerable<PageLocalisationDbEntity> pageLocalisations)
        {
            var currentPageLocalisations = context.PageLocalisations
                .AsNoTracking()
                .Where(x => x.PageId == pageId)
                .ToList();

            foreach (var currentPageLocalisation in currentPageLocalisations)
            {
                var pageLocalisation = pageLocalisations
                    .FirstOrDefault(x => x.LanguageId == currentPageLocalisation.LanguageId);

                if (pageLocalisation == null)
                    context.Remove(currentPageLocalisation);
            }

            foreach (var pageLocalisation in pageLocalisations)
            {
                var currentPageLocalisation = context.PageLocalisations
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.PageId == pageLocalisation.PageId &&
                        x.LanguageId == pageLocalisation.LanguageId);

                if (currentPageLocalisation == null)
                    context.Add(pageLocalisation);
                else
                    context.Entry(pageLocalisation).State = EntityState.Modified;
            }
        }

        private void UpdatePageModules(WeapsyDbContext context, IEnumerable<PageModuleDbEntity> pageModules)
        {
            foreach (var pageModule in pageModules)
            {
                var currentPageModule = context.PageModules
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.ModuleId == pageModule.ModuleId &&
                        x.PageId == pageModule.PageId);

                if (currentPageModule == null)
                {
                    context.Add(pageModule);
                }
                else
                {
                    context.Entry(pageModule).State = EntityState.Modified;
                    UpdatePageModuleLocalisations(context, pageModule.Id, pageModule.PageModuleLocalisations);
                    UpdatePageModulePermissions(context, pageModule.Id, pageModule.PageModulePermissions);
                }
            }
        }

        private void UpdatePageModuleLocalisations(WeapsyDbContext context, Guid pageModuleId, IEnumerable<PageModuleLocalisationDbEntity> pageModuleLocalisations)
        {
            var currentPageModuleLocalisations = context.PageModuleLocalisations
                .AsNoTracking()
                .Where(x => x.PageModuleId == pageModuleId)
                .ToList();

            foreach (var currentPageModuleLocalisation in currentPageModuleLocalisations)
            {
                var pageModuleLocalisation = pageModuleLocalisations
                    .FirstOrDefault(x => x.PageModuleId == currentPageModuleLocalisation.PageModuleId
                    && x.LanguageId == currentPageModuleLocalisation.LanguageId);

                if (pageModuleLocalisation == null)
                    context.Remove(currentPageModuleLocalisation);
            }

            foreach (var pageModuleLocalisation in pageModuleLocalisations)
            {
                var currentPageModuleLocalisation = context.PageModuleLocalisations
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.PageModuleId == pageModuleLocalisation.PageModuleId &&
                        x.LanguageId == pageModuleLocalisation.LanguageId);

                if (currentPageModuleLocalisation == null)
                    context.Add(pageModuleLocalisation);
                else
                    context.Entry(pageModuleLocalisation).State = EntityState.Modified;
            }
        }

        private void UpdatePageModulePermissions(WeapsyDbContext context, Guid pageModuleId, IEnumerable<PageModulePermissionDbEntity> pageModulePermissions)
        {
            var currentPageModulePermissions = context.PageModulePermissions
                .AsNoTracking()
                .Where(x => x.PageModuleId == pageModuleId)
                .ToList();

            foreach (var currentPageModulePermission in currentPageModulePermissions)
            {
                var pageModulePermission = pageModulePermissions
                    .FirstOrDefault(x => x.PageModuleId == currentPageModulePermission.PageModuleId
                    && x.RoleId == currentPageModulePermission.RoleId
                    && x.Type == currentPageModulePermission.Type);

                if (pageModulePermission == null)
                    context.Remove(currentPageModulePermission);
            }

            foreach (var pageModulePermission in pageModulePermissions)
            {
                var currentPageModulePermission = currentPageModulePermissions
                    .FirstOrDefault(x => x.PageModuleId == pageModulePermission.PageModuleId
                    && x.RoleId == pageModulePermission.RoleId
                    && x.Type == pageModulePermission.Type);

                if (currentPageModulePermission == null)
                    context.Add(pageModulePermission);
            }
        }

        private void UpdatePagePermissions(WeapsyDbContext context, Guid pageId, IEnumerable<PagePermissionDbEntity> pagePermissions)
        {
            var currentPagePermissions = context.PagePermissions
                .AsNoTracking()
                .Where(x => x.PageId == pageId)
                .ToList();

            foreach (var currentPagePermission in currentPagePermissions)
            {
                var pagePermission = pagePermissions
                    .FirstOrDefault(x => x.PageId == currentPagePermission.PageId
                    && x.RoleId == currentPagePermission.RoleId
                    && x.Type == currentPagePermission.Type);

                if (pagePermission == null)
                    context.Remove(currentPagePermission);
            }

            foreach (var pagePermission in pagePermissions)
            {
                var existingPagePermissionDbEntity = currentPagePermissions
                    .FirstOrDefault(x => x.PageId == pagePermission.PageId
                    && x.RoleId == pagePermission.RoleId
                    && x.Type == pagePermission.Type);

                if (existingPagePermissionDbEntity == null)
                    context.Add(pagePermission);
            }
        }

        private void LoadActivePageModules(WeapsyDbContext context, PageDbEntity pageDbEntity)
        {
            if (pageDbEntity == null)
                return;

            pageDbEntity.PageModules = context.PageModules
                .Include(y => y.PageModuleLocalisations)
                .Include(y => y.PageModulePermissions)
                .Where(x => x.PageId == pageDbEntity.Id && x.Status != PageModuleStatus.Deleted)
                .ToList();
        }
    }
}