using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories
{
    public class VisitRequestRepository : IVisitRequestRepository
    {
        private readonly IApplicationDbContext _context;

        public VisitRequestRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(VisitRequest entity, CancellationToken cancellationToken = default)
        {
            await _context.VisitRequests.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // --- اصلاح شد: ساخت فیلتر به روش صحیح ---
            var filter = Builders<VisitRequest>.Filter.Eq(c => c.Id, id);
            await _context.VisitRequests.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<VisitRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VisitRequests.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<VisitRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // متد Find عبارت لامبدا را قبول می‌کند، بنابراین این بخش صحیح است
            return await _context.VisitRequests.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(VisitRequest entity, CancellationToken cancellationToken = default)
        {
            // --- اصلاح شد: ساخت فیلتر به روش صحیح ---
            var filter = Builders<VisitRequest>.Filter.Eq(c => c.Id, entity.Id);
            await _context.VisitRequests.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }
        public async Task<IEnumerable<VisitRequest>> GetByAgentIdAsync(Guid agentId)
        {
            return await _context.VisitRequests.Find(x => x.AgentId==agentId).SortByDescending(x => x.RequestDate).ToListAsync();

        }

        public async Task<long> CountPendingAsync()
        {
            return await _context.VisitRequests.CountDocumentsAsync(r => r.Status=="Pending");
        }

        public async Task<long> CountAsync(Expression<Func<VisitRequest, bool>> filter)
        {
            return await _context.VisitRequests.CountDocumentsAsync(filter);
        }

        public async Task<IEnumerable<VisitRequest>> GetSomeAsync<TKey>(Expression<Func<VisitRequest, bool>> filter, Expression<Func<VisitRequest, TKey>> keySelector, int count, bool descending = true)
        {
            var sortDefinition = new ExpressionFieldDefinition<VisitRequest, TKey>(keySelector);
            var sort = descending
                ? Builders<VisitRequest>.Sort.Descending(sortDefinition)
                : Builders<VisitRequest>.Sort.Ascending(sortDefinition);

            return await _context.VisitRequests.Find(filter).Sort(sort).Limit(count).ToListAsync();
        }
    }
}
