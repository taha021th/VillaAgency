using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IApplicationDbContext _context;
        public CategoryRepository(IApplicationDbContext context)
        {
            _context= context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Find(_ => true).ToListAsync();
        }
        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Categories.Find(c => c.Id==id).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            await _context.Categories.ReplaceOneAsync(c => c.Id==category.Id, category);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Categories.DeleteOneAsync(c => c.Id==id);
        }

        public async Task<long> CountAsync(Expression<Func<Category, bool>> filter)
        {
            return await _context.Categories.CountDocumentsAsync(filter);
        }
    }
}
