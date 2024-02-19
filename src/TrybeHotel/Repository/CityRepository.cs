using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            var allcities = _context.Cities.ToList();
            return allcities.Select(city => new CityDto {
                cityId = city.CityId,
                name = city.Name,
                state = city.State
            });
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            var newcity = _context.Cities.Add(city);
            _context.SaveChanges();
            return new CityDto {
                cityId = newcity.Entity.CityId,
                name = newcity.Entity.Name,
                state = newcity.Entity.State
            };
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var updateCity = _context.Cities.Update(city);
            _context.SaveChanges();
            return new CityDto {
                cityId = updateCity.Entity.CityId,
                name = updateCity.Entity.Name,
                state = updateCity.Entity.State
            };
        }

    }
}