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
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [Authorize]
        [HttpPost("/addUser")]
        public async Task<ActionResult> AddUser([FromBody] UserFormDTO user)
        {
            Claim? userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                return NotFound(new { message = "Can't find ID in user token." });
            }
            

            if (user == null) {
                return BadRequest(new {message = "User cannot be null."});
            }

            if (await _userRepository.GetUserInfoById(userClaim.Value) != null)
            {
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
                return Ok(new { mesage = "User created sucessfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                return NotFound(new { message = "Can't find ID in user token." });
            }

            User? user = await _userRepository.GetUserInfoById(userClaim.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);

        }
    }

}
