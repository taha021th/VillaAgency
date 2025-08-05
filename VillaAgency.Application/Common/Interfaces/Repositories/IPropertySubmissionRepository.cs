using System.Linq.Expressions;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories
{
    public interface IPropertySubmissionRepository : IRepository<PropertySubmission>
    {

        Task<IEnumerable<PropertySubmission>> GetSomeAsync<TKey>(Expression<Func<PropertySubmission, bool>> filter, Expression<Func<PropertySubmission, TKey>> keySelector, int count, bool descending = true);
        Task<IEnumerable<PropertySubmission>> FindAsync(Expression<Func<PropertySubmission, bool>> predicate);
        Task<long> CountAsync(Expression<Func<PropertySubmission, bool>> filter);
    }
}
