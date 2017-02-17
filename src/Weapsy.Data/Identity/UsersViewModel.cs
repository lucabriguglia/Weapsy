using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Data.Entities;

namespace Weapsy.Data.Identity
{
    public class UsersViewModel
    {
        public IList<User> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
