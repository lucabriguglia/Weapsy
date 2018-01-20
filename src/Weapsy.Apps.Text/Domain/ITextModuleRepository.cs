using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain
{
    public interface ITextModuleRepository : IRepository<TextModule>
    {
        TextModule GetById(Guid id);
        TextModule GetByModuleId(Guid moduleId);
        void Create(TextModule text);
        void Update(TextModule text);
    }
}
