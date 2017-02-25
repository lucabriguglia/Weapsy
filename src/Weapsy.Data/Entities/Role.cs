using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Data.Entities
{
    public class Role : IdentityRole<Guid>, IDbEntity
    {
    }
}
