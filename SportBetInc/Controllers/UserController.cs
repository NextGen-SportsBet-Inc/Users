using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportBetInc.Models;
using SportBetInc.Models.DTOs;
using SportBetInc.Repositories;
using System.Security.Claims;

namespace SportBetInc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserRepository userRepository, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger _logger = logger;

        [Authorize]
        [HttpPost("/addUser")]
        public async Task<ActionResult> AddUser([FromBody] UserFormDTO user)
        {
            Claim? userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                _logger.LogWarning("Couldn't find user ID from token.");
                return NotFound(new { message = "Can't find ID in user token." });
            }

            if (user == null) {
                _logger.LogWarning("User from body is null.");
                return BadRequest(new {message = "User cannot be null."});
            }

            if (await _userRepository.GetUserInfoById(userClaim.Value) != null)
            {
                _logger.LogWarning("There's already a user with ID {userId}.", userClaim.Value);
                return Conflict(new { message = "A user with that ID already exists."});
            }

            try
            {
                var user_ = new User() { 
                    FirstName = user.FirstName, 
                    Email = user.Email, 
                    LastName = user.LastName, 
                    Id = userClaim.Value
                };
                
                await _userRepository.AddUserToDbAsync(user_);
                _logger.LogInformation("Added user to repository with ID {userID}.", userClaim.Value);
                return Ok(new { mesage = "User created sucessfully." });
            }
            catch (Exception ex)
            {
                _logger.LogWarning("New exception was found: {ex}", ex.Message);
                return StatusCode(500, new { mesage = "Error creating user.", details = ex.ToString() });
            }
        }


        [Authorize]
        [HttpGet("/getUserInfo")]
        public async Task<ActionResult<User>> GetUserInfo()
        {
            Claim? userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                _logger.LogWarning("Couldn't find user ID from token.");
                return NotFound(new { message = "Can't find ID in user token." });
            }

            User? user = await _userRepository.GetUserInfoById(userClaim.Value);
            if (user == null)
            {
                _logger.LogWarning("User found is null.");
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);

        }

        
    }

}
