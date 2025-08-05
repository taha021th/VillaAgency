using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories
{
    public class PropertySubmissionRepository : IPropertySubmissionRepository
    {
        private readonly IApplicationDbContext _context;

        public PropertySubmissionRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PropertySubmission entity, CancellationToken cancellationToken)
        {
            await _context.PropertySubmissions.InsertOneAsync(entity);
        }


        public async Task<long> CountAsync(Expression<Func<PropertySubmission, bool>> filter)
        {
            return await _context.PropertySubmissions.CountDocumentsAsync(filter);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PropertySubmission>> FindAsync(Expression<Func<PropertySubmission, bool>> predicate)
        {
            return await _context.PropertySubmissions.Find(predicate).ToListAsync();

        }

        public async Task<IEnumerable<PropertySubmission>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.PropertySubmissions.Find(_ => true).SortByDescending(s => s.SubmittedAt).ToListAsync();
        }

        public async Task<PropertySubmission> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<PropertySubmission>.Filter.Eq(p => p.Id, id);
            return await _context.PropertySubmissions.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<PropertySubmission>> GetSomeAsync<TKey>(Expression<Func<PropertySubmission, bool>> filter, Expression<Func<PropertySubmission, TKey>> keySelector, int count, bool descending = true)
        {

            var sortDefinition = new ExpressionFieldDefinition<PropertySubmission, TKey>(keySelector);
            var sort = descending
                ? Builders<PropertySubmission>.Sort.Descending(sortDefinition)
                : Builders<PropertySubmission>.Sort.Ascending(sortDefinition);

            return await _context.PropertySubmissions.Find(filter).Sort(sort).Limit(count).ToListAsync();

        }

        public async Task UpdateAsync(PropertySubmission entity, CancellationToken cancellationToken)
        {
            await _context.PropertySubmissions.ReplaceOneAsync(s => s.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }
    }
}
