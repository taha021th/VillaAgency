using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertyRequests.Commands
{
    public record CreatePropertyRequestCommand : IRequest<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string TransactionType { get; set; }
        public List<Guid> CategoryIds { get; set; } = new();
        public string DesiredRegions { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }
        public int? MinBedrooms { get; set; }
        public string? Description { get; set; }
    }
    public class CreatePropertyRequestCommandHandler : IRequestHandler<CreatePropertyRequestCommand, Guid>
    {
        private readonly IPropertyRequestRepository _propertyRequestRepository;
        public CreatePropertyRequestCommandHandler(IPropertyRequestRepository propertyRequestRepository)
        {
            _propertyRequestRepository = propertyRequestRepository;
        }

        public async Task<Guid> Handle(CreatePropertyRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new PropertyRequest
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                TransactionType = request.TransactionType,
                CategoryIds = request.CategoryIds,
                DesiredRegions = request.DesiredRegions,
                MinBudget = request.MinBudget,
                MaxBudget = request.MaxBudget,
                MinArea = request.MinArea,
                MaxArea = request.MaxArea,
                MinBedrooms = request.MinBedrooms,
                Description = request.Description

            };
            await _propertyRequestRepository.AddAsync(entity, cancellationToken);
            return entity.Id;
        }
    }
}
