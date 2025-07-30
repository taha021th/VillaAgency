using System.Linq.Expressions;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories
{
    public interface IPropertySubmissionRepository : IRepository<PropertySubmission>
    {
        Task<IEnumerable<PropertySubmission>> FindAsync(Expression<Func<PropertySubmission, bool>> predicate);
    }
}
