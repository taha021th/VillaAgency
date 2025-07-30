using System.Linq.Expressions;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories
{
    public interface IVisitRequestRepository : IRepository<VisitRequest>
    {
        Task<IEnumerable<VisitRequest>> GetByAgentIdAsync(Guid agentId);
        Task<long> CountPendingAsync();
        Task<long> CountAsync(Expression<Func<VisitRequest, bool>> filter);
        Task<IEnumerable<VisitRequest>> GetSomeAsync<TKey>(Expression<Func<VisitRequest, bool>> filter, Expression<Func<VisitRequest, TKey>> keySelector, int count, bool descending = true);
    }
}
