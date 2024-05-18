using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportBetInc.Models;
using SportBetInc.Repositories;

namespace SportBetInc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet("/getAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var userInfo = await _userRepository.GetAllUsersInfo();

            return Ok(userInfo);
        }
    }
}
