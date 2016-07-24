using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Weapsy.Services.Identity
{
    public class UsersViewModel
    {
        public IList<IdentityUser> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
