using System;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Menus.Rules
{
    public class MenuRules : IMenuRules
    {
        private readonly IMenuRepository _menuRepository;

        public MenuRules(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public bool IsMenuIdUnique(Guid id)
        {
            return _menuRepository.GetById(id) == null;
        }

        public bool IsMenuNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^[A-Za-z\d\s_-]+$");
            var match = regex.Match(name);
            return match.Success;
        }

        public bool IsMenuNameUnique(Guid siteId, string name)
        {
            var menu = _menuRepository.GetByName(siteId, name);
            return menu == null || menu.Status == MenuStatus.Deleted;
        }
    }
}