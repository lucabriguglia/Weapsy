using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Themes
{
    public interface IThemeRepository : IRepository<Theme>
    {
        Theme GetById(Guid id);
        Theme GetByName(string name);
        Theme GetByFolder(string folder);
        int GetThemesCount();        
        void Create(Theme theme);
        void Update(Theme theme);
        void Update(IEnumerable<Theme> themes);
    }
}
