using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Users
{
    public class UsersAdminViewModel
    {
        public IEnumerable<UserAdminListModel> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
