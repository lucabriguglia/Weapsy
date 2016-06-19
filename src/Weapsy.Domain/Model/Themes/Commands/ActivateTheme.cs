using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes.Commands
{
    public class ActivateTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
