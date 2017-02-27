using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Users
{
    public class UserAdminModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }

        public IEnumerable<string> AllRoles { get; set; } = new List<string>();
        public IEnumerable<string> UserRoles { get; set; } = new List<string>();
    }
}
