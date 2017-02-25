using System.Collections.Generic;

namespace Weapsy.Reporting.Users
{
    public class UsersAdminViewModel
    {
        public IList<UserAdminModel> Users { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
