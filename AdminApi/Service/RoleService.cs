using AdminApi.Context.DomainModel;
using AdminApi.IService;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork work;

        public RoleService(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<bool> AddAsync(Role model)
        {
            await work.GetRepository<Role>().InsertAsync(model);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repository = work.GetRepository<Role>();
            var role = await repository
                .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(role);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<List<Role>> GetAllAsync(QueryParameter parameter)
        {
            var repository = work.GetRepository<Role>();
            var roles = await repository.GetPagedListAsync(null,
               pageIndex: parameter.PageIndex,
               pageSize: parameter.PageSize,
               orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return (List<Role>)roles;
        }

        public async Task<Role> GetSingleAsync(int id)
        {
            var repository = work.GetRepository<Role>();
            var role = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            return role;
        }

        public async Task<bool> UpdateAsync(Role model)
        {

            var repository = work.GetRepository<Role>();
            var role = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(model.Id));

            role.Name = model.Name;
            role.Description = model.Description;
            role.UpdateDate = DateTime.Now;

            repository.Update(role);

            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
