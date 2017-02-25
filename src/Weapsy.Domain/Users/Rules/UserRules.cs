using System;

namespace Weapsy.Domain.Users.Rules
{
    public class UserRules : IUserRules
    {
        private readonly IUserRepository _userRepository;

        public UserRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsUserNameUnique(string name, Guid userId = new Guid())
        {
            var user = _userRepository.GetByUserName(name);
            return IsUserUnique(user, userId);
        }

        public bool IsUserEmailUnique(string email, Guid userId = new Guid())
        {
            var user = _userRepository.GetByEmail(email);
            return IsUserUnique(user, userId);
        }

        private bool IsUserUnique(User user, Guid userId)
        {
            return user == null
                || user.Status == UserStatus.Deleted
                || (userId != Guid.Empty && user.Id == userId);
        }
    }
}