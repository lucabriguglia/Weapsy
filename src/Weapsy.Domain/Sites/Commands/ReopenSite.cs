using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Sites.Commands
{
    public class ReopenSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
