using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class AddressRepo(UserDataContext context) : BaseRepo<AddressEntity, UserDataContext>(context)
{
}