using System;
using System.Threading.Tasks;

namespace Weapsy.Apps.Text.Domain
{
    public interface ITextModuleRepository
    {
        TextModule GetById(Guid id);
        TextModule GetByModuleId(Guid moduleId);
        Task CreateAsync(TextModule text);
        void Update(TextModule text);
    }
}
