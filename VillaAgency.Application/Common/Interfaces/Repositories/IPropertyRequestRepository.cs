using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories
{
    public interface IPropertyRequestRepository
    {
        Task<IEnumerable<PropertyRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task<PropertyRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(PropertyRequest entity, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, PropertyRequest entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<(IEnumerable<PropertyRequest> Requests, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize);
    }
}
