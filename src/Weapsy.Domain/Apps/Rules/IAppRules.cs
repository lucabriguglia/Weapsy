using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Apps.Rules
{
    public interface IAppRules : IRules<App>
    {
        bool DoesAppExist(Guid id);
        bool IsAppIdUnique(Guid id);
        bool IsAppNameUnique(string name, Guid appId = new Guid());
        bool IsAppFolderValid(string folder);
        bool IsAppFolderUnique(string folder, Guid appId = new Guid());
    }
}
