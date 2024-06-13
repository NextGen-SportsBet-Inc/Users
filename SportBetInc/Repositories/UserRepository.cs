using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportBetInc.Models;

namespace SportBetInc.Repositories
{
    public class UserRepository(UsersDbContext userContext) : IUserRepository
    {
        private readonly UsersDbContext _userContext = userContext;

        public virtual async Task AddUserToDbAsync(User user)
        {
            await _userContext.Users.AddAsync(user);
            await _userContext.SaveChangesAsync();
        }

        public virtual async Task<User?> GetUserInfoById(String id)
        {
            return await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
