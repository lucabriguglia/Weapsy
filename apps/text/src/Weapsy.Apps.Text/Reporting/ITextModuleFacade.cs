using System;

namespace Weapsy.Apps.Text.Reporting
{
    public interface ITextModuleFacade
    {
        string GetContent(Guid moduleId, Guid languageId = new Guid());
        TextModuleAdminModel GetAdminModel(Guid siteId, Guid moduleId, Guid versionId = new Guid());
    }
}
