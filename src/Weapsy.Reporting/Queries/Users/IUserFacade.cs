using System;

namespace Weapsy.Reporting.Users
{
    public interface IUserFacade
    {
        UserDto GeById(Guid id);        
    }
}
