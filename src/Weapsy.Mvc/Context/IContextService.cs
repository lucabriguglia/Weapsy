using Weapsy.Reporting.Sites;

namespace Weapsy.Mvc.Context
{
    public interface IContextService
    {
        ContextInfo GetCurrentContextInfo();
    }
}
