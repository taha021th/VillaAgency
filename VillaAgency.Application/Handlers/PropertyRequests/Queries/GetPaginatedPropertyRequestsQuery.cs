using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertyRequests.Queries
{
    public record GetPaginatedPropertyRequestsQuery : IRequest<(IEnumerable<PropertyRequest> Requests, int TotalCount)>
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPaginatedPropertyRequestsQueryHandler : IRequestHandler<GetPaginatedPropertyRequestsQuery, (IEnumerable<PropertyRequest>, int)>
    {
        private readonly IPropertyRequestRepository _repository;

        public GetPaginatedPropertyRequestsQueryHandler(IPropertyRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<PropertyRequest>, int)> Handle(GetPaginatedPropertyRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllPaginatedAsync(request.PageNum, request.PageSize);
        }
    }
}
