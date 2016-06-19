using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Commands
{
    public class CreateSite : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
