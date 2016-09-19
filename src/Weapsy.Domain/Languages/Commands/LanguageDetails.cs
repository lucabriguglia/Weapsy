using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Languages.Commands
{
    public class LanguageDetails : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
    }
}
