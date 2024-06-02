using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProvider.Infrastructure.Data.Contexts;
using UserProvider.Infrastructure.Data.Entities;

namespace UserProvider.Infrastructure.Data.Repositories
{
    public class UserRepository(IDbContextFactory<UserDataContext> contextFactory, ILogger<BaseRepository<ApplicationUser, UserDataContext>> logger) : BaseRepository<ApplicationUser, UserDataContext>(contextFactory, logger)
    {
		private readonly IDbContextFactory<UserDataContext> _contextFactory = contextFactory;

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersWithAddressesAsync()
        {
			try
			{
				using var context = _contextFactory.CreateDbContext();
				return await context.Users
					.Include(u => u.UserAddresses).ThenInclude(ua => ua.Address)
					.Include(u => u.UserAddresses).ThenInclude(ua => ua.OptionalAddress)
					.ToListAsync();
			}
			catch (Exception)
			{

				return [];
			}
        }


    }
}
