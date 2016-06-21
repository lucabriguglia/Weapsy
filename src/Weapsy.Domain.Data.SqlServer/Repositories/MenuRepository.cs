using System;
using System.Linq;
using Weapsy.Domain.Model.Menus;
using MenuDbEntity = Weapsy.Domain.Data.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.Entities.MenuItemLocalisation;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Weapsy.Domain.Data.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<MenuDbEntity> _menus;
        private readonly DbSet<MenuItemDbEntity> _menuItems;
        private readonly DbSet<MenuItemLocalisationDbEntity> _menuItemLocalisations;
        private readonly IMapper _mapper;

        public MenuRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _menus = context.Set<MenuDbEntity>();
            _menuItems = context.Set<MenuItemDbEntity>();
            _menuItemLocalisations = context.Set<MenuItemLocalisationDbEntity>();
            _mapper = mapper;
        }

        public Menu GetById(Guid id)
        {
            var dbEntity = _menus.FirstOrDefault(x => x.Id == id);
            LoadMenuItems(dbEntity);
            return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
        }

        public Menu GetById(Guid siteId, Guid id)
        {
            var dbEntity = _menus.FirstOrDefault(x => x.SiteId == siteId &&  x.Id == id);
            LoadMenuItems(dbEntity);
            return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
        }

        public Menu GetByName(Guid siteId, string name)
        {
            var dbEntity = _menus.FirstOrDefault(x => x.SiteId == siteId && x.Name == name);
            LoadMenuItems(dbEntity);
            return dbEntity != null ? _mapper.Map<Menu>(dbEntity) : null;
        }

        public ICollection<Menu> GetAll(Guid siteId)
        {
            var dbEntities = _menus.Where(x => x.SiteId == siteId && x.Status != MenuStatus.Deleted).ToList();
            foreach (var dbEntity in dbEntities) LoadMenuItems(dbEntity);
            return _mapper.Map<ICollection<Menu>>(dbEntities);
        }

        public void Create(Menu menu)
        {
            var menuDbEntity = _mapper.Map<MenuDbEntity>(menu);
            _menus.Add(menuDbEntity);
            _context.SaveChanges();
        }

        public void Update(Menu menu)
        {
            var menuDbEntity = _menus.FirstOrDefault(x => x.SiteId == menu.SiteId && x.Id == menu.Id);

            //menuDbEntity = _mapper.Map(menu, menuDbEntity);
            menuDbEntity.Name = menu.Name;
            menuDbEntity.Status = menu.Status;

            UpdateMenuItems(menu.MenuItems);

            _context.SaveChanges();
        }

        private void UpdateMenuItems(IEnumerable<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                var menuItemDbEntity = _menuItems.FirstOrDefault(x => x.Id == menuItem.Id);

                if (menuItemDbEntity == null)
                {
                    _menuItems.Add(_mapper.Map<MenuItemDbEntity>(menuItem));
                }
                else
                {
                    //menuItemDbEntity = _mapper.Map(menuItem, menuItemDbEntity);
                    menuItemDbEntity.Link = menuItem.Link;
                    menuItemDbEntity.Status = menuItem.Status;
                    menuItemDbEntity.MenuItemType = menuItem.MenuItemType;
                    menuItemDbEntity.PageId = menuItem.PageId;
                    menuItemDbEntity.ParentId = menuItem.ParentId;
                    menuItemDbEntity.SortOrder = menuItem.SortOrder;
                    menuItemDbEntity.Text = menuItem.Text;
                    menuItemDbEntity.Title = menuItem.Title;

                    UpdateMenuItemLocalisations(menuItem.MenuItemLocalisations);
                }
            }
        }

        public void UpdateMenuItemLocalisations(ICollection<MenuItemLocalisation> menuItemLocalisations)
        {
            foreach (var menuItemLocalisation in menuItemLocalisations)
            {
                var menuItemLocalisationDbEntity = _menuItemLocalisations.FirstOrDefault(x => x.MenuItemId == menuItemLocalisation.MenuItemId && x.LanguageId == menuItemLocalisation.LanguageId);

                if (menuItemLocalisationDbEntity == null)
                {
                    _menuItemLocalisations.Add(_mapper.Map<MenuItemLocalisationDbEntity>(menuItemLocalisation));
                }
                else
                {
                    //menuItemLocalisationDbEntity = _mapper.Map(menuItemLocalisation, menuItemLocalisationDbEntity);
                    menuItemLocalisationDbEntity.LanguageId = menuItemLocalisation.LanguageId;
                    menuItemLocalisationDbEntity.MenuItemId = menuItemLocalisation.MenuItemId;
                    menuItemLocalisationDbEntity.Text = menuItemLocalisation.Text;
                    menuItemLocalisationDbEntity.Title = menuItemLocalisation.Title;
                }
            }
        }

        private void LoadMenuItems(MenuDbEntity menu)
        {
            if (menu != null)
            {
                menu.MenuItems = _menuItems
                    .Include(x => x.MenuItemLocalisations)
                    .Where(x => x.MenuId == menu.Id && x.Status != MenuItemStatus.Deleted)
                    .ToList();
            }
        }

        //public void Update(Menu menu)
        //{
        //    var menuDbEntity = _menus
        //        .AsNoTracking()
        //        .Include(x => x.MenuItems)
        //        .ThenInclude(y => y.MenuItemLocalisations)
        //        .FirstOrDefault(x => x.SiteId == menu.SiteId && x.Id == menu.Id);

        //    _menus.Update(menu.ToDbEntity(menuDbEntity));

        //    foreach (var menuItem in menu.MenuItems)
        //    {
        //        var menuItemDbEntity = _menuItems.FirstOrDefault(x => x.Id == menuItem.Id);

        //        if (menuItemDbEntity == null)
        //        {
        //            _menuItems.Add(menuItem.ToDbEntity());
        //        }
        //        else
        //        {
        //            _menuItems.Update(menuItem.ToDbEntity(menuItemDbEntity));
        //        }

        //        foreach (var menuItemLocalisation in menuItem.MenuItemLocalisations)
        //        {
        //            var menuItemLocalisationDbEntity = _menuItemLocalisations.FirstOrDefault(x => x.MenuItemId == menuItemLocalisation.MenuItemId && x.LanguageId == menuItemLocalisation.LanguageId);

        //            if (menuItemLocalisationDbEntity == null)
        //            {
        //                _menuItemLocalisations.Add(menuItemLocalisation.ToDbEntity());
        //            }
        //            else
        //            {
        //                _menuItemLocalisations.Update(menuItemLocalisation.ToDbEntity(menuItemLocalisationDbEntity));
        //            }
        //        }
        //    }

        //    _context.SaveChanges();
        //}
    }
}
