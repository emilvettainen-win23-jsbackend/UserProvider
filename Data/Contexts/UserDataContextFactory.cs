﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Contexts;

public class UserDataContextFactory : IDesignTimeDbContextFactory<UserDataContext>
{
    public UserDataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDataContext>();
        optionsBuilder.UseSqlServer("Server=tcp:evettainen.database.windows.net,1433;Initial Catalog=Silicon-SqlServer;Persist Security Info=False;User ID=SqlAdmin;Password=ZWQ3OTM1MzUtMDA3Ni00YmRhLWIyOGQtZmEzY2ZkZDkyMTcx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        return new UserDataContext(optionsBuilder.Options);
    }
}
