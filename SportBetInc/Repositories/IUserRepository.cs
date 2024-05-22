using SportBetInc.Models;
using SportBetInc.Models.DTOs;

namespace SportBetInc.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersInfo();
        Task AddUserToDbAsync(User user);

        Task<User?> GetUserInfoById(String id);
    }
}
