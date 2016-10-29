using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Menus;
using MenuDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Repositories
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
                var dbEntity = context.Set<MenuDbEntity>().FirstOrDefault(x => x.Id == id);
                LoadMenuItems(context, dbEntity);
                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public Menu GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<MenuDbEntity>().FirstOrDefault(x => x.SiteId == siteId &&  x.Id == id);
                LoadMenuItems(context, dbEntity);
                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public Menu GetByName(Guid siteId, string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<MenuDbEntity>().FirstOrDefault(x => x.SiteId == siteId && x.Name == name);
                LoadMenuItems(context, dbEntity);
                return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
            }
        }

        public ICollection<Menu> GetAll(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<MenuDbEntity>().Where(x => x.SiteId == siteId && x.Status != MenuStatus.Deleted).ToList();
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
                var menuDbEntity = context.Set<MenuDbEntity>()
                    .Include(x => x.MenuItems)
                    .ThenInclude(y => y.MenuItemLocalisations)
                    .FirstOrDefault(x => x.Id == menu.Id);

                menuDbEntity.Name = menu.Name;
                menuDbEntity.Status = menu.Status;

                foreach (var menuItem in menu.MenuItems)
                {
                    var menuItemDbEntity = menuDbEntity.MenuItems.FirstOrDefault(x => x.Id == menuItem.Id);

                    if (menuItemDbEntity == null)
                    {
                        var newMenItemDbEntity = _mapper.Map<MenuItemDbEntity>(menuItem);
                        menuDbEntity.MenuItems.Add(newMenItemDbEntity);
                    }
                    else
                    {
                        menuItemDbEntity.Link = menuItem.Link;
                        menuItemDbEntity.Status = menuItem.Status;
                        menuItemDbEntity.MenuItemType = menuItem.MenuItemType;
                        menuItemDbEntity.PageId = menuItem.PageId;
                        menuItemDbEntity.ParentId = menuItem.ParentId;
                        menuItemDbEntity.SortOrder = menuItem.SortOrder;
                        menuItemDbEntity.Text = menuItem.Text;
                        menuItemDbEntity.Title = menuItem.Title;                        
                    }

                    foreach (var menuItemLocalisation in menuItem.MenuItemLocalisations)
                    {
                        var menuItemLocalisationDbEntity = menuItemDbEntity.MenuItemLocalisations.FirstOrDefault(x => x.MenuItemId == menuItemLocalisation.MenuItemId && x.LanguageId == menuItemLocalisation.LanguageId);

                        if (menuItemLocalisationDbEntity == null)
                        {
                            var newMenItemLocalisationDbEntity = _mapper.Map<MenuItemLocalisationDbEntity>(menuItem);
                            menuItemDbEntity.MenuItemLocalisations.Add(newMenItemLocalisationDbEntity);
                        }
                        else
                        {
                            menuItemLocalisationDbEntity.LanguageId = menuItemLocalisation.LanguageId;
                            menuItemLocalisationDbEntity.MenuItemId = menuItemLocalisation.MenuItemId;
                            menuItemLocalisationDbEntity.Text = menuItemLocalisation.Text;
                            menuItemLocalisationDbEntity.Title = menuItemLocalisation.Title;
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private void LoadMenuItems(WeapsyDbContext context, MenuDbEntity menu)
        {
            if (menu == null)
                return;

            menu.MenuItems = context.Set<MenuItemDbEntity>()
                .Include(x => x.MenuItemLocalisations)
                .Where(x => x.MenuId == menu.Id && x.Status != MenuItemStatus.Deleted)
                .ToList();
        }
    }
}
