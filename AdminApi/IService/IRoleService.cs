using AdminApi.Context.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.IService
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<List<Role>> GetAllRolesAsync();
    }
}
