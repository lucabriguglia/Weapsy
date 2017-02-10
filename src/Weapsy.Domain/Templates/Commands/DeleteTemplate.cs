using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Templates.Commands
{
    public class DeleteTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
