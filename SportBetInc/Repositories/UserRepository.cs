using Microsoft.EntityFrameworkCore;
using SportBetInc.Models;

namespace SportBetInc.Repositories
{
    public class UserRepository(UsersDbContext userRepository) : IUserRepository
    {
        private readonly UsersDbContext _userRepository = userRepository;

        public virtual async Task<List<User>> GetAllUsersInfo()
        {
            return await _userRepository.Users.ToListAsync();
        }


    }
}
