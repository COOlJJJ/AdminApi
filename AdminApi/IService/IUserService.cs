using AdminApi.Context.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.IService
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetSingleAsync(string Account, string Password);
    }
}
