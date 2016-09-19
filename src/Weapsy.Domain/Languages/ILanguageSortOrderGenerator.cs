using System;

namespace Weapsy.Domain.Languages
{
    public interface ILanguageSortOrderGenerator
    {
        int GenerateNextSortOrder(Guid siteId);
    }
}
