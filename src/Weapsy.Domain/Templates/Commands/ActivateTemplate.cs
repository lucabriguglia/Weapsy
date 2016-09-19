using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class ActivateTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
