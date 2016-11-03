using System;

namespace Weapsy.Domain.Languages.Commands
{
    public class LanguageDetails : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
    }
}
