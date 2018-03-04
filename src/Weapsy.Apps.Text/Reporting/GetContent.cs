using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Apps.Text.Reporting
{
    public class GetContent : IQuery
    {
        public Guid ModuleId { get; set; }
        public Guid LanguageId { get; set; }
    }
}
