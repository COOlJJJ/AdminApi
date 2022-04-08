using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminApi.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<PagedList<T>> GetAllAsync(QueryParameter query);

        Task<T> GetSingleAsync(int id);

        Task<bool> AddAsync(T model);

        Task<bool> UpdateAsync(T model);

        Task<bool> DeleteAsync(int id);
    }
}
