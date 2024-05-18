using SportBetInc.Models;

namespace SportBetInc.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersInfo();
    }
}
