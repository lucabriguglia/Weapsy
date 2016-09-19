using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Commands
{
    public class ReopenSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
