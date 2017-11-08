using System;

namespace Weapsy.Domain.Languages.Commands
{
    public class DeleteLanguageCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
