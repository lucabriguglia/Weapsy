using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Pages;
using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
using PageLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;
using PagePermissionDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PagePermission;
using PageModulePermissionDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModulePermission;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public PageRepository(IWeapsyDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Page GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<PageDbEntity>()
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.Id == id);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Page GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<PageDbEntity>()
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.SiteId == siteId &&  x.Id == id);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Page GetByName(Guid siteId, string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<PageDbEntity>()
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.SiteId == siteId && x.Name == name);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Page GetByUrl(Guid siteId, string url)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<PageDbEntity>()
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefault(x => x.SiteId == siteId && x.Url == url);

                LoadActivePageModules(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Page>(dbEntity) : null;
            }
        }

        public Guid? GetIdBySlug(Guid siteId, string slug)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<PageDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.Url == slug && x.Status == PageStatus.Active);
                return dbEntity?.Id;
            }
        }

        public ICollection<Page> GetAll(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<PageDbEntity>()
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .Where(x => x.SiteId == siteId)
                    .OrderBy(x => x.Name).ToList();

                foreach (var dbEntity in dbEntities)
                    LoadActivePageModules(context, dbEntity);

                return _mapper.Map<ICollection<Page>>(dbEntities);
            }
        }

        public void Create(Page page)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<PageDbEntity>(page);
                context.Set<PageDbEntity>().Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Page page)
        {
            using (var context = _dbContextFactory.Create())
            {
                var pageDbEntity = _mapper.Map<PageDbEntity>(page);

                context.Entry(pageDbEntity).State = EntityState.Modified;

                UpdatePageLocalisations(context, pageDbEntity.PageLocalisations);
                UpdatePageModules(context, pageDbEntity.PageModules);
                UpdatePagePermissions(context, pageDbEntity.Id, pageDbEntity.PagePermissions);

                context.SaveChanges();
            }
        }

        private void UpdatePageLocalisations(WeapsyDbContext context, IEnumerable<PageLocalisationDbEntity> pageLocalisations)
        {
            foreach (var pageLocalisation in pageLocalisations)
            {
                var currentPageLocalisation = context.Set<PageLocalisationDbEntity>()
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.PageId == pageLocalisation.PageId &&
                        x.LanguageId == pageLocalisation.LanguageId);

                if (currentPageLocalisation == null)
                {
                    context.Add(pageLocalisation);
                }
                else
                {
                    context.Entry(pageLocalisation).State = EntityState.Modified;
                }
            }
        }

        private void UpdatePageModules(WeapsyDbContext context, IEnumerable<PageModuleDbEntity> pageModules)
        {
            foreach (var pageModule in pageModules)
            {
                var currentPageModule = context.Set<PageModuleDbEntity>()
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
                    UpdatePageModuleLocalisations(context, pageModule.PageModuleLocalisations);
                    UpdatePageModulePermissions(context, pageModule.Id, pageModule.PageModulePermissions);
                }
            }
        }

        private void UpdatePageModuleLocalisations(WeapsyDbContext context, IEnumerable<PageModuleLocalisationDbEntity> pageModuleLocalisations)
        {
            foreach (var pageModuleLocalisation in pageModuleLocalisations)
            {
                var currentPageModuleLocalisation = context.Set<PageModuleLocalisationDbEntity>()
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.PageModuleId == pageModuleLocalisation.PageModuleId &&
                        x.LanguageId == pageModuleLocalisation.LanguageId);

                if (currentPageModuleLocalisation == null)
                {
                    context.Add(pageModuleLocalisation);
                }
                else
                {
                    context.Entry(pageModuleLocalisation).State = EntityState.Modified;
                }
            }
        }

        private void UpdatePageModulePermissions(WeapsyDbContext context, Guid pageModuleId, IEnumerable<PageModulePermissionDbEntity> pageModulePermissions)
        {
            var currentPageModulePermissions = context.Set<PageModulePermissionDbEntity>()
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
            var currentPagePermissions = context.Set<PagePermissionDbEntity>()
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

            pageDbEntity.PageModules = context.Set<PageModuleDbEntity>()
                .Include(y => y.PageModuleLocalisations)
                .Include(y => y.PageModulePermissions)
                .Where(x => x.PageId == pageDbEntity.Id && x.Status != PageModuleStatus.Deleted)
                .ToList();
        }
    }
}