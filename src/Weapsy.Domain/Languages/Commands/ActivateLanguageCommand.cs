using System;

namespace Weapsy.Domain.Languages.Commands
{
    public class ActivateLanguageCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
