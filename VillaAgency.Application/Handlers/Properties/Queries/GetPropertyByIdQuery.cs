using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record GetPropertyByIdQuery(Guid Id) : IRequest<Property>;


    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, Property>
    {
        private readonly IPropertyRepository _propertyRepository;
        public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<Property> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _propertyRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
