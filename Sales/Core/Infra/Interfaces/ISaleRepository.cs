using Sales.Core.Domain.Entities;

namespace Sales.Core.Infra.Interfaces
{
    public interface ISaleRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }  
}
