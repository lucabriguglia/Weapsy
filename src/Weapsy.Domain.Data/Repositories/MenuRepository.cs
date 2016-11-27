using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Menus;
using MenuDbEntity = Weapsy.Data.Entities.Menu;
using MenuItemDbEntity = Weapsy.Data.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Data.Entities.MenuItemLocalisation;
using MenuItemPermissionDbEntity = Weapsy.Data.Entities.MenuItemPermission;

namespace Weapsy.Domain.Data.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public MenuRepository(IWeapsyDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Menu GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<MenuDbEntity>()
                    .FirstOrDefault(x => x.Id == id && x.Status != MenuStatus.Deleted);

                LoadMenuItems(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public Menu GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<MenuDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId &&  x.Id == id && x.Status != MenuStatus.Deleted);

                LoadMenuItems(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public Menu GetByName(Guid siteId, string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<MenuDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.Name == name && x.Status != MenuStatus.Deleted);

                LoadMenuItems(context, dbEntity);

                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public ICollection<Menu> GetAll(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<MenuDbEntity>()
                    .Where(x => x.SiteId == siteId && x.Status != MenuStatus.Deleted)
                    .ToList();

                foreach (var dbEntity in dbEntities)
                    LoadMenuItems(context, dbEntity);

                return _mapper.Map<ICollection<Menu>>(dbEntities);
            }
        }

        public void Create(Menu menu)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<MenuDbEntity>(menu);
                context.Set<MenuDbEntity>().Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Menu menu)
        {
            using (var context = _dbContextFactory.Create())
            {
                var menuDbEntity = _mapper.Map<MenuDbEntity>(menu);

                context.Entry(menuDbEntity).State = EntityState.Modified;

                UpdateMenuItems(context, menuDbEntity.MenuItems);

                context.SaveChanges();
            }
        }

        private void UpdateMenuItems(WeapsyDbContext context, IEnumerable<MenuItemDbEntity> menuItemDbEntities)
        {
            foreach (var menuItemDbEntity in menuItemDbEntities)
            {
                var currentMenuItem = context.MenuItems.AsNoTracking().FirstOrDefault(x => x.Id == menuItemDbEntity.Id);

                if (currentMenuItem == null)
                {
                    context.Add(menuItemDbEntity);
                }
                else
                {
                    context.Entry(menuItemDbEntity).State = EntityState.Modified;
                    UpdateMenuItemLocalisations(context, menuItemDbEntity.MenuItemLocalisations);
                    UpdateMenuItemPermissions(context, currentMenuItem.Id, menuItemDbEntity.MenuItemPermissions);
                }
            }
        }

        private void UpdateMenuItemLocalisations(WeapsyDbContext context, IEnumerable<MenuItemLocalisationDbEntity> menuItemLocalisationDbEntities)
        {
            foreach (var menuItemLocalisationDbEntity in menuItemLocalisationDbEntities)
            {
                var currentMenuItemLocalisation = context.MenuItemLocalisations.AsNoTracking()
                    .FirstOrDefault(x => x.MenuItemId == menuItemLocalisationDbEntity.MenuItemId 
                    && x.LanguageId == menuItemLocalisationDbEntity.LanguageId);

                if (currentMenuItemLocalisation == null)
                {
                    context.Add(menuItemLocalisationDbEntity);
                }
                else
                {
                    context.Entry(menuItemLocalisationDbEntity).State = EntityState.Modified;
                }                
            }
        }

        private void UpdateMenuItemPermissions(WeapsyDbContext context, Guid menuItemId, IEnumerable<MenuItemPermissionDbEntity> menuItemPermissions)
        {
            var currentMenuItemPermissions = context.Set<MenuItemPermissionDbEntity>()
                .AsNoTracking()
                .Where(x => x.MenuItemId == menuItemId)
                .ToList();

            foreach (var currentMenuItemPermission in currentMenuItemPermissions)
            {
                var menuItemPermission = menuItemPermissions
                    .FirstOrDefault(x => x.MenuItemId == currentMenuItemPermission.MenuItemId
                    && x.RoleId == currentMenuItemPermission.RoleId);

                if (menuItemPermission == null)
                    context.Remove(currentMenuItemPermission);
            }

            foreach (var menuItemPermission in menuItemPermissions)
            {
                var currentPageModulePermission = currentMenuItemPermissions
                    .FirstOrDefault(x => x.MenuItemId == menuItemPermission.MenuItemId
                    && x.RoleId == menuItemPermission.RoleId);

                if (currentPageModulePermission == null)
                    context.Add(menuItemPermission);
            }
        }

        private void LoadMenuItems(WeapsyDbContext context, MenuDbEntity menu)
        {
            if (menu == null)
                return;

            menu.MenuItems = context.Set<MenuItemDbEntity>()
                .Include(x => x.MenuItemLocalisations)
                .Include(x => x.MenuItemPermissions)
                .Where(x => x.MenuId == menu.Id && x.Status != MenuItemStatus.Deleted)
                .ToList();
        }
    }
}
