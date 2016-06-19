using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Templates.Commands
{
    public class DeleteTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
