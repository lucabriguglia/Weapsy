using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class DeleteTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
