using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LocalCellars.API.Data;
using LocalCellars.API.Dtos;
using LocalCellars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LocalCellars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
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

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            
            if (userFromRepo == null)
                return Unauthorized();
            //Claims array that we can store the ID and the Username in that the server grabs from
            //the JWT Token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            //The key to sign our token with/ this key will be hashed
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            //After we create a security key above then we're using this
            //Key as part of the signing creds and encrypting this key w/ a hashing algo
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //This is where we start to actually create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };
            //This handler allows us to create the token based on the 
            //token descriptor being passed to the create token method
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //We use the handlers to write the token into a response sent back to
            //the client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}