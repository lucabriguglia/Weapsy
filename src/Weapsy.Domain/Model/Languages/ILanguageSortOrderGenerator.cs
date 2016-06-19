using System;

namespace Weapsy.Domain.Model.Languages
{
    public interface ILanguageSortOrderGenerator
    {
        int GenerateNextSortOrder(Guid siteId);
    }
}
