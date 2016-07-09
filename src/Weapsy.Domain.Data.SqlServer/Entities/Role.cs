using System;

namespace Weapsy.Domain.Data.SqlServer.Entities
{
    public class Role : IDbEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
