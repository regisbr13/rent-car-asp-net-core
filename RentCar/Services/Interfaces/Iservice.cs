using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentCar.Services.Interfaces
{
    public interface Iservice<TEntity> where TEntity : class
    {
        Task<List<TEntity>> FindAllAsync();
        Task<TEntity> FindByIdAsync(string id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(string id);
    }
}
