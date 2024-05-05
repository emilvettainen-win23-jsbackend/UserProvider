using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using UserProvider.Data.Entities;
using UserProvider.Models;

namespace UserProvider.Services;

public class UserService
{
	private readonly UserManager<UserEntity> _userManager;

    public UserService(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CreateUserAsync(CreateUserModel model)
    {
		try
		{
			var isFirstUser = !await _userManager.Users.AnyAsync(x => !x.IsExternalAccount);
			var userExists = await _userManager.Users.AnyAsync(x => x.Email == model.Email);
			if (userExists)
			{
				return false;
			}
			var userEntity = new UserEntity { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Created = DateTime.Now };
			var createUser = await _userManager.CreateAsync(userEntity, model.Password);
            if (!createUser.Succeeded)
            {
				return false;
            }
			var standardRole = isFirstUser ? "Admin" : "User";
			var roleResult = await _userManager.AddToRoleAsync(userEntity, standardRole);
			return roleResult.Succeeded ? true : false;
        }
		catch (Exception)
		{

			return false;
		}
    }
}
