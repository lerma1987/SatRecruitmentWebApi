using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;
namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SatRecruitmentContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(SatRecruitmentContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }
        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }
        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task AddByRange(IEnumerable<T> entity)
        {
            await _entities.AddRangeAsync(entity);
        }
        public void Update(T entity)
        {
            _entities.Update(entity);
        }
        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}
