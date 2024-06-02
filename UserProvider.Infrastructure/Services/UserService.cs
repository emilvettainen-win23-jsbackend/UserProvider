using Microsoft.Extensions.Logging;
using UserProvider.Infrastructure.Data.Repositories;
using UserProvider.Infrastructure.Factories;
using UserProvider.Infrastructure.Helpers.Responses;

namespace UserProvider.Infrastructure.Services;

public class UserService(UserRepository userRepository, ILogger<UserService> logger)
{
	private readonly UserRepository _userRepository = userRepository;
	private readonly ILogger<UserService> _logger = logger;

    public async Task<ResponseResult> GetAllUsersAsync()
    {
		try
		{
			var users = await _userRepository.GetAllUsersWithAddressesAsync();
			return users.Any() ? ResponseFactory.Ok(users.Select(UserFactory.Read)) : ResponseFactory.NotFound();
		}
		catch (Exception ex)
		{
            _logger.LogError($"ERROR : UserService.GetAllUsersAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
		}
    }
}