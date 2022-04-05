using AdminApi.Context.DomainModel;
using AdminApi.IService;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Service
{
    public class UserRoleService : IUserRoleService
    {
        public readonly IUnitOfWork work;

        public UserRoleService(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<bool> AddAsync(UserRole model)
        {
            await work.GetRepository<UserRole>().InsertAsync(model);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repository = work.GetRepository<UserRole>();
            var userRole = await repository
                .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(userRole);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<List<UserRole>> GetAllAsync(QueryParameter parameter)
        {
            var repository = work.GetRepository<UserRole>();
            var userRoles = await repository.GetPagedListAsync(null,
               pageIndex: parameter.PageIndex,
               pageSize: parameter.PageSize,
               orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return (List<UserRole>)userRoles;
        }

        public async Task<UserRole> GetSingleAsync(int id)
        {
            var repository = work.GetRepository<UserRole>();
            var userRole = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            return userRole;
        }

        public async Task<bool> UpdateAsync(UserRole model)
        {
            var repository = work.GetRepository<UserRole>();
            var userRole = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(model.Id));

            userRole.RoleID = model.RoleID;
            userRole.UserID = model.UserID;
            userRole.UpdateDate = DateTime.Now;

            repository.Update(userRole);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<string> GetUserAllRolesName(int UserId)
        {
            var repository = work.GetRepository<UserRole>();
            var userRoles = await repository.GetAllAsync(predicate: x => x.UserID == UserId);
            string roles = string.Empty;
            foreach (var item in userRoles)
            {
                var role = await work.GetRepository<Role>().GetFirstOrDefaultAsync(predicate: x => x.Id == item.RoleID);
                roles += role.Name + ",";
            }
            if (!string.IsNullOrEmpty(roles))
            {
                roles = roles[0..^1];
            }
            return roles;
        }
    }
}
