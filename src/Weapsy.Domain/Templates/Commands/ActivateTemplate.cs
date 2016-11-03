using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class ActivateTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
