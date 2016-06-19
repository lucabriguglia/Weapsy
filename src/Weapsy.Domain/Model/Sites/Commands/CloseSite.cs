using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Commands
{
    public class CloseSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
