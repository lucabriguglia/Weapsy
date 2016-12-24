using System;
using Weapsy.Apps.Text.Domain.Commands;

namespace Weapsy.Apps.Text.Reporting
{
    public interface ITextModuleFacade
    {
        string GetContent(Guid moduleId, Guid languageId = new Guid());
        AddVersion GetAdminModel(Guid siteId, Guid moduleId, Guid versionId = new Guid());
    }
}
