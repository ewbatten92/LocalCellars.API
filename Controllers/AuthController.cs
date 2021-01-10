using System.Threading.Tasks;
using LocalCellars.API.Data;
using LocalCellars.API.Dtos;
using LocalCellars.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocalCellars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request
            //Set  Username to lowercase
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            //Check if username exists in db
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");
            // if not then create a new user to pass to the register method
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            //pass user thru register method and return a created user in db
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            //return 201 created back to client for HTTP request
            return StatusCode(201);
        }
    }
}