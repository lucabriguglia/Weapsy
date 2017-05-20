using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class DeleteTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
