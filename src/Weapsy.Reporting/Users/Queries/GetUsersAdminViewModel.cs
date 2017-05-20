using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Users.Queries
{
    public class GetUsersAdminViewModel : IQuery
    {
        public int StartIndex { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
