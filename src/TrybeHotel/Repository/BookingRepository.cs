using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var user = _context.Users.First(u => u.Email == email);
            var room = GetRoomById(booking.RoomId);

            var newBooking = new Booking
            {
                CheckIn = DateTime.Parse(booking.CheckIn),
                CheckOut = DateTime.Parse(booking.CheckOut),
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = user.UserId
            };

            _context.Bookings.Add(newBooking);

            _context.SaveChanges();

            var hotel = _context.Hotels.First(h => h.HotelId == room.HotelId);

            return new BookingResponse
            {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        hotelId = hotel.HotelId,
                        name = hotel.Name,
                        address = hotel.Address,
                        cityId = hotel.CityId,
                        cityName = (from city in _context.Cities where city.CityId == hotel.CityId select city.Name).First(),
                        state = (from city in _context.Cities where city.CityId == hotel.CityId select city.State).First(),
                    }
                }
            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
           var user = _context.Users.First(usr => usr.Email == email);

           var bookspe = _context.Bookings.FirstOrDefault(bk => bk.BookingId == bookingId);
            if (user == null || bookspe == null || bookspe.UserId != user.UserId)
            {
                return null;
            }

            var room = GetRoomById(bookspe.RoomId);
            var hotel = _context.Hotels.First(h => h.HotelId == room.HotelId);

            return new BookingResponse {
                BookingId = bookspe.BookingId,
                CheckIn = bookspe.CheckIn,
                CheckOut = bookspe.CheckOut,
                GuestQuant = bookspe.GuestQuant,
                Room = new RoomDto {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto {
                        hotelId = hotel.HotelId,
                        name = hotel.Name,
                        address = hotel.Address,
                        cityId = hotel.CityId,
                        cityName = ( from city in _context.Cities where city.CityId == hotel.CityId select city.Name).First(),
                        state = ( from city in _context.Cities where city.CityId == hotel.CityId select city.State).First(),
                    },
                },
            };
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);

            return room;
        }

    }

}