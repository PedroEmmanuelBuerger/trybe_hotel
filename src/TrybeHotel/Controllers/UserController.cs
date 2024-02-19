using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers(){
             try
            {
                var users = _repository.GetUsers();
                return StatusCode(200, users);
            }
            catch
            {
                return StatusCode(401);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
             var existEmail = _repository.GetUserByEmail(user.Email);
            if (existEmail != null)
            {
                return StatusCode(409, new { message = "User email already exists" });
            }

            var newUser = _repository.Add(user);
            return StatusCode(201, newUser);
        }
    }
}