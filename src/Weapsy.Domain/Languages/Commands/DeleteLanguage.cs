using System;

namespace Weapsy.Domain.Languages.Commands
{
    public class DeleteLanguage : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
