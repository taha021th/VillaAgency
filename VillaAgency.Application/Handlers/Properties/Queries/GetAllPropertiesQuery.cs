using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record GetAllPropertiesQuery : IRequest<IEnumerable<Property>>;

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IEnumerable<Property>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<Property>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await _propertyRepository.GetAllAsync(cancellationToken);
        }
    }

}
