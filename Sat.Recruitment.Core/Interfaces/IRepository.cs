using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task AddByRange(IEnumerable<T> entity);
        void Update(T entity);
        Task Delete(int id);
    }
}
