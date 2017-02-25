using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Domain.Users;

namespace Weapsy.Data.Entities
{
    public class User : IdentityUser<Guid>, IDbEntity
    {
        public UserStatus Status { get; set; }
    }
}
