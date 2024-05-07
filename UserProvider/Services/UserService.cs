using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using UserProvider.Factories;
using UserProvider.Helpers.Responses;
using UserProvider.Models;

namespace UserProvider.Services;

public class UserService
{
	private readonly UserManager<UserEntity> _userManager;
	private readonly ILogger<UserService> _logger;
	private readonly IHttpClientFactory _httpClientFactory;

    public UserService(UserManager<UserEntity> userManager, ILogger<UserService> logger, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
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


	public async Task<ResponseResult> VerifyUserAsync(VerifyUserModel model)
	{
		try
		{
			var client = _httpClientFactory.CreateClient();
			var response = await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("VerificationURI"), model);
			if (!response.IsSuccessStatusCode)
				return ResponseFactory.UnAuthorized();

			return await UpdateUserComfirmedEmailAsync(model.Email) ? ResponseFactory.Ok(): ResponseFactory.Error();
		}
		catch (Exception ex)
		{
            _logger.LogError($"ERROR : UserService.VerifyUserAsync() :: {ex.Message}");
            return ResponseFactory.Error();
		}
	}



	public async Task<bool> UpdateUserComfirmedEmailAsync(string email)
	{
		try
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return false;

			user.EmailConfirmed = true;
			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded && await _userManager.IsEmailConfirmedAsync(user);
		}
		catch (Exception ex)
		{
            _logger.LogError($"ERROR : UserService.UpdateUserComfirmedEmailAsync() :: {ex.Message}");
            return false;
        }
	}
}