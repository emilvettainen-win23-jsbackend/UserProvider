using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserProvider.Data.Entities;

namespace UserProvider.Data.Contexts;

public class UserDataContext(DbContextOptions<UserDataContext> options) : IdentityDbContext<UserEntity>(options)
{

}
