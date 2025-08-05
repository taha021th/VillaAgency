using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories
{
    public class PropertyRequestRepository : IPropertyRequestRepository
    {
        private readonly IApplicationDbContext _context;
        public PropertyRequestRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PropertyRequest entity, CancellationToken cancellationToken)
        {
            await _context.PropertyRequests.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<long> CountAsync(Expression<Func<PropertyRequest, bool>> filter)
        {
            return await _context.PropertyRequests.CountDocumentsAsync(filter);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _context.PropertyRequests.FindOneAndDeleteAsync(x => x.Id==id);
        }

        public async Task<IEnumerable<PropertyRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.PropertyRequests.Find(_ => true).SortByDescending(x => x.SubmittedAt).ToListAsync();
        }

        public async Task<(IEnumerable<PropertyRequest> Requests, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var count = await _context.PropertyRequests.CountDocumentsAsync(_ => true);
            var items = await _context.PropertyRequests.Find(_ => true)
                .SortByDescending(p => p.SubmittedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return (items, (int)count);
        }

        public async Task<PropertyRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.PropertyRequests.Find(x => x.Id==id).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<PropertyRequest>> GetSomeAsync<TKey>(Expression<Func<PropertyRequest, bool>> filter, Expression<Func<PropertyRequest, TKey>> keySelector, int count, bool descending = true)
        {
            var sortDefinition = new ExpressionFieldDefinition<PropertyRequest, TKey>(keySelector);
            var sort = descending
                ? Builders<PropertyRequest>.Sort.Descending(sortDefinition)
                : Builders<PropertyRequest>.Sort.Ascending(sortDefinition);

            return await _context.PropertyRequests.Find(filter).Sort(sort).Limit(count).ToListAsync();
        }

        public async Task UpdateAsync(Guid id, PropertyRequest entity, CancellationToken cancellationToken)
        {
            await _context.PropertyRequests.ReplaceOneAsync(x => x.Id == id, entity, cancellationToken: cancellationToken);

        }
    }
}
