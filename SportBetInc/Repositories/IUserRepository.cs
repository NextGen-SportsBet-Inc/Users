using SportBetInc.Models;

namespace SportBetInc.Repositories
{
    public interface IUserRepository
    {
        Task AddUserToDbAsync(User user);

        Task<User?> GetUserInfoById(String id);

        void RemoveUser(User user);
    }
}
