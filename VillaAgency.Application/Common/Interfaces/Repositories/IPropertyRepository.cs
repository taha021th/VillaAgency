using System.Linq.Expressions;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories;


public interface IPropertyRepository : IRepository<Property>
{
    Task<IEnumerable<Property>> GetPropertiesByAgentIdAsync(Guid agentId);
    Task<long> CountAsync(Expression<Func<Property, bool>> filter);
    Task<IEnumerable<Property>> GetSomeAsync<TKey>(Expression<Func<Property, bool>> filter, Expression<Func<Property, TKey>> keySelector, int count, bool descending = true);
    Task<(IEnumerable<Property> Properties, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Property?>> FindAsync(Expression<Func<Property, bool>> predicate, CancellationToken cancellationToken);
    Task<Property?> FindOneAsync(Expression<Func<Property, bool>> predicate, CancellationToken cancellationToken);

    Task<(IEnumerable<Property> Properties, int TotalCount)> SearchAndPaginateAsync(
        string? searchTerm,
        Guid? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int? minArea,
        int? maxArea,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}

