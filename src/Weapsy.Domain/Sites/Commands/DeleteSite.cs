using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Commands
{
    public class DeleteSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
