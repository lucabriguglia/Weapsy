using System;
using Weapsy.Domain.Users;

namespace Weapsy.Data.Entities
{
    public class User : IDbEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserStatus Status { get; set; }
    }
}
