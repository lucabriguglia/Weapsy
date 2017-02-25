using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Users
{
    public class UserRolesViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public IList<string> AvailableRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
