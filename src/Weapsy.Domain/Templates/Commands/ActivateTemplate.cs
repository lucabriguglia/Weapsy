using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Templates.Commands
{
    public class ActivateTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
