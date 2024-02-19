using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var allrooms = from room in _context.Rooms
                           where room.HotelId == HotelId
                           select new RoomDto
                           {
                               RoomId = room.RoomId,
                               Name = room.Name,
                               Image = room.Image,
                               Capacity = room.Capacity,
                               Hotel = new HotelDto
                               {
                                   hotelId = room.Hotel.HotelId,
                                   name = room.Hotel.Name,
                                   address = room.Hotel.Address,
                                   cityId = room.Hotel.CityId,
                                   cityName = room.Hotel.City.Name,
                                   state = room.Hotel.City.State
                               },
                           };
            return allrooms.ToList();
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            var newRoom = _context.Rooms.Add(room);
            _context.SaveChanges();

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Image = room.Image,
                Capacity = room.Capacity,
                Hotel = (from hotel in _context.Hotels where hotel.HotelId == room.HotelId select new HotelDto
                {
                    hotelId = hotel.HotelId,
                    name = hotel.Name,
                    address = hotel.Address,
                    cityId = hotel.CityId,
                    cityName = (from city in _context.Cities where city.CityId == hotel.CityId select city.Name).First(),
                    state = (from city in _context.Cities where city.CityId == hotel.CityId select city.State).First(),
                }).First()
            };
        }

        public void DeleteRoom(int RoomId)
        {
            throw new NotImplementedException();
        }
    }
}