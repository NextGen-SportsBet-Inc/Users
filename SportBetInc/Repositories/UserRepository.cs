using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportBetInc.Models;

namespace SportBetInc.Repositories
{
    public class UserRepository(UsersDbContext userRepository) : IUserRepository
    {
        private readonly UsersDbContext _userRepository = userRepository;

        public virtual async Task AddUserToDbAsync(User user)
        {
            await _userRepository.Users.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public virtual async Task<User?> GetUserInfoById(String id)
        {
            return await _userRepository.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual void RemoveUser(User user)
        {
            _userRepository.Users.Remove(user);
        }

    }
}
