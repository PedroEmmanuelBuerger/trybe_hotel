using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
        var allHotel = from hotel in _context.Hotels
                           select new HotelDto
                           {
                               hotelId = hotel.HotelId,
                               name = hotel.Name,
                               address = hotel.Address,
                               cityId = hotel.CityId,
                               cityName = hotel.City.Name,
                               state = hotel.City.State
                           };
            return allHotel.ToList();
        }
        
        public HotelDto AddHotel(Hotel hotel)
        {
            var newHotel = _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var response = new HotelDto {
                    hotelId = newHotel.Entity.HotelId,
                    name = newHotel.Entity.Name,
                    address = newHotel.Entity.Address,
                    cityId = newHotel.Entity.CityId,
                    cityName = (from city in _context.Cities where city.CityId == newHotel.Entity.CityId select city.Name).First(),
                    state = (from city in _context.Cities where city.CityId == newHotel.Entity.CityId select city.State).First(),
                };

            return response;
        }
    }
}