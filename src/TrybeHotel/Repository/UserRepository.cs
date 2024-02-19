using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
             var user = _context.Users.Where(u => u.Email == login.Email && u.Password == login.Password).Select(u => new UserDto
           {
               UserId = u.UserId,
               Name = u.Name,
               Email = u.Email,
               UserType = u.UserType
           });

           if (user.Count() == 0)
           {
               return null;
           }

              return user.First();
        }
        public UserDto Add(UserDtoInsert user)
        {
             var newUser =  new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
             var user = _context.Users.Where(u => u.Email == userEmail).Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            });

            if (user.Count() == 0)
            {
                return null;
            }
        return user.First();
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            });

            return users.ToList();
        }

    }
}