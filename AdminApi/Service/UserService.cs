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

        public async Task<PagedList<User>> GetAllAsync(QueryParameter parameter)
        {
            var repository = work.GetRepository<User>();
            var reuslt = await repository.GetPagedListAsync(
                predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.UserName.Contains(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderBy(t => t.Id));
            return (PagedList<User>)reuslt;
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
                             (x.PassWord.Equals(Password)) && x.Status == "1");
            return user;
        }

        public async Task<bool> UpdateAsync(User model)
        {

            var repository = work.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(model.Id));

            user.ReMark = model.ReMark;
            user.UpdateDate = DateTime.Now;
            repository.Update(user);

            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> UpdatePasswordAsync(int userid, string password)
        {

            var repository = work.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(userid));

            user.PassWord = password;
            user.UpdateDate = DateTime.Now;
            repository.Update(user);

            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> UpdateStatusAsync(int userid, string status)
        {
            var repository = work.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(userid));

            user.Status = status;
            user.UpdateDate = DateTime.Now;
            repository.Update(user);

            if (await work.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
