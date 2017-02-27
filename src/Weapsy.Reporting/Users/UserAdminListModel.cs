using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Users
{
    public class UserAdminListModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
