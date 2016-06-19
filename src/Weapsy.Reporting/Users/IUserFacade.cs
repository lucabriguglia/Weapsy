using System;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Users
{
    public interface IUserFacade
    {
        Task<UserDto> GeById(Guid Id);        
    }
}
