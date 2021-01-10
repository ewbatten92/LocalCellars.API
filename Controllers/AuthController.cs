using LocalCellars.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace LocalCellars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
    }
}