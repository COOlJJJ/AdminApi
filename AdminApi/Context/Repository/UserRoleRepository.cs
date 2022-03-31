using AdminApi.Context.DomainModel;

namespace AdminApi.Context.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IRepository<UserRole>
    {
        public UserRoleRepository(AdminContext dbContext) : base(dbContext)
        {
        }
    }
}
