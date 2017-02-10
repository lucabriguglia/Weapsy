using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class DeleteTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
