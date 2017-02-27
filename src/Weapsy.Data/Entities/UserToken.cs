using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Data.Entities
{
    public class UserToken : IdentityUserToken<Guid>
    {
    }
}
