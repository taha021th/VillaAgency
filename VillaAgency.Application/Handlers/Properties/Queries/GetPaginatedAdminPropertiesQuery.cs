

using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public class GetPaginatedAdminPropertiesQuery : IRequest<(IEnumerable<Property> properties, int TotalCount)>
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPaginatedAdminPropertiesQueryHandler : IRequestHandler<GetPaginatedAdminPropertiesQuery, (IEnumerable<Property>, int)>
    {

        private readonly IPropertyRepository _propertyRepository;
        public GetPaginatedAdminPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<(IEnumerable<Property>, int)> Handle(GetPaginatedAdminPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await _propertyRepository.GetAllPaginatedAsync(request.PageNum, request.PageSize);
        }
    }
}
