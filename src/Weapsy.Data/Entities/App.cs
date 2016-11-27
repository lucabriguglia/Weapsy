using System;
using Weapsy.Domain.Apps;

namespace Weapsy.Data.Entities
{
    public class App : IDbEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public AppStatus Status { get; set; }
    }
}