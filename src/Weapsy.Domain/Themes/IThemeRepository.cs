using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes
{
    public interface IThemeRepository : IRepository<Theme>
    {
        Theme GetById(Guid id);
        Theme GetByName(string name);
        Theme GetByFolder(string folder);
        ICollection<Theme> GetAll();
        int GetThemesCount();        
        void Create(Theme theme);
        void Update(Theme theme);
        void Update(IEnumerable<Theme> themes);
    }
}
