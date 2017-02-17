using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Weapsy.Data.Entities;

namespace Weapsy.Services.Identity
{
    public class UsersViewModel
    {
        public IList<User> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
