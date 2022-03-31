using AdminApi.Context.DomainModel;
using AdminApi.IService;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork work;

        public UserService(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<bool> AddAsync(User model)
        {
            await work.GetRepository<User>().InsertAsync(model);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repository = work.GetRepository<User>();
            var user = await repository
                .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(user);
            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<List<User>> GetAllAsync(QueryParameter parameter)
        {
            var repository = work.GetRepository<User>();
            var users = await repository.GetPagedListAsync(null,
               pageIndex: parameter.PageIndex,
               pageSize: parameter.PageSize,
               orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return (List<User>)users;
        }

        public async Task<User> GetSingleAsync(int id)
        {
            var repository = work.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            return user;
        }

        public async Task<User> GetSingleAsync(string Account, string Password)
        {
            var user = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate:
                             x => (x.Account.Equals(Account)) &&
                             (x.PassWord.Equals(Password)) && x.Status == 1);
            return user;
        }

        public async Task<bool> UpdateAsync(User model)
        {

            var repository = work.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(model.Id));

            user.UserName = model.UserName;
            user.UpdateDate = DateTime.Now;

            repository.Update(user);

            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
