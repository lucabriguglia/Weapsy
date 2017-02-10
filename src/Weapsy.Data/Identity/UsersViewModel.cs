using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Data.Identity
{
    public class UsersViewModel
    {
        public IList<IdentityUser> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
