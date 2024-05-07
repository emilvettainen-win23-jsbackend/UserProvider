using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using UserProvider.Data.Entities;
using UserProvider.Factories;
using UserProvider.Helpers.Responses;
using UserProvider.Models;

namespace UserProvider.Services;

public class UserService
{
	private readonly UserManager<UserEntity> _userManager;
	private readonly ILogger<UserService> _logger;

    public UserService(UserManager<UserEntity> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ResponseResult> CreateUserAsync(CreateUserModel model)
    {
		try
		{
			var isFirstUser = !await _userManager.Users.AnyAsync(x => !x.IsExternalAccount);
			var userExists = await _userManager.Users.AnyAsync(x => x.Email == model.Email);
			if (userExists)
			{
				return ResponseFactory.Exists();
			}
			var userEntity = new UserEntity { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Created = DateTime.Now };
			var createUser = await _userManager.CreateAsync(userEntity, model.Password);
            if (!createUser.Succeeded)
            {
				return ResponseFactory.Error();
            }
			var standardRole = isFirstUser ? "Admin" : "User";
			var roleResult = await _userManager.AddToRoleAsync(userEntity, standardRole);
			return roleResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
		catch (Exception ex)
		{
            _logger.LogError($"ERROR : UserService.CreateUserAsync() :: {ex.Message}");
            return ResponseFactory.Error();
		}
    }
}
