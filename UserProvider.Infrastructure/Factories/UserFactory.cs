using UserProvider.Infrastructure.Data.Entities;
using UserProvider.Infrastructure.Dtos;

namespace UserProvider.Infrastructure.Factories
{
    public class UserFactory
    {
        public static UserDto Read (ApplicationUser user)
        {
            return new UserDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Biography = user.Biography,
                PhoneNumber = user.PhoneNumber,
                Addresses = user.UserAddresses.Select(ua => new AddressDto
                {
                    StreetName = ua.Address?.StreetName,
                    PostalCode = ua.Address?.PostalCode,
                    City = ua.Address?.City,
                    OptionalAddress = ua.OptionalAddress?.OptionalAddress
                }).ToList()
            };
        }
    }
}
