using System;
using Weapsy.Domain.Model.Apps;

namespace Weapsy.Domain.Data.Entities
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