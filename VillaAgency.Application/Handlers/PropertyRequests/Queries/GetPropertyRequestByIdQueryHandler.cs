using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertyRequests.Queries
{
    public record GetPropertyRequestByIdQuery : IRequest<PropertyRequest>
    {
        public Guid Id { get; init; }
    }
    public class GetPropertyRequestByIdQueryHandler : IRequestHandler<GetPropertyRequestByIdQuery, PropertyRequest>
    {
        private readonly IPropertyRequestRepository _propertyRequestRepository;
        public GetPropertyRequestByIdQueryHandler(IPropertyRequestRepository propertyRequestRepository)
        {
            _propertyRequestRepository = propertyRequestRepository;
        }
        public async Task<PropertyRequest> Handle(GetPropertyRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _propertyRequestRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                throw new Exception("Property request not found.");
            }
            return result;
        }
    }
}
