using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Commands
{
    public class DeleteSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
