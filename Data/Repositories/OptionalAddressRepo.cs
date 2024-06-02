using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class OptionalAddressRepo(UserDataContext context) : BaseRepo<OptionalAddressEntity, UserDataContext>(context)
{
}
