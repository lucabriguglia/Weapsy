using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes.Commands
{
    public class DeleteTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
