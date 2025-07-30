using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public class GetPropertiesByAgentIdQuery : IRequest<IEnumerable<Property>>
    {
        public Guid AgentId { get; set; }
    }
    public class GetPropertiesByAgentIdQueryHandler : IRequestHandler<GetPropertiesByAgentIdQuery, IEnumerable<Property>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesByAgentIdQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<Property>> Handle(GetPropertiesByAgentIdQuery request, CancellationToken cancellationToken)
        {
            return await _propertyRepository.GetPropertiesByAgentIdAsync(request.AgentId);
        }
    }
}
