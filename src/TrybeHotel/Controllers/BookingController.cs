using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
             var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var booking = _repository.Add(bookingInsert, email);

            var room = _repository.GetRoomById(bookingInsert.RoomId);
            if (bookingInsert.GuestQuant > room.Capacity)
            {
                return StatusCode(400, new { message = "Guest quantity over room capacity" });
            }

            return StatusCode(201, booking);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
             var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            try
            {
                var getBooking = _repository.GetBooking(Bookingid, email);
                if (getBooking == null)
                {
                    return StatusCode(401, new { message = "Booking not found" });
                }
                    
                return StatusCode(200, getBooking);
            }
            catch {
                return StatusCode(401);
            }
        }
    }
}