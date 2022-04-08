using AdminApi.Context.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminApi.IService
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        Task<string> GetUserAllRolesName(int UserId);

        Task<List<string>> GetUserAllRolesNameList(int UserId);

        Task<bool> GiveRoles(int userid, List<string> roles);
    }
}
