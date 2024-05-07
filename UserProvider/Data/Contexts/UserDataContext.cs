using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserProvider.Data.Entities;

namespace UserProvider.Data.Contexts;

public class UserDataContext : IdentityDbContext<UserEntity>
{
    public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
    {
    }
}