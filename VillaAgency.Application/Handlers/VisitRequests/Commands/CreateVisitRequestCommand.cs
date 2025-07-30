using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.VisitRequests.Commands
{
    public record CreateVisitRequestCommand : IRequest<Guid>
    {
        public Guid PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public Guid AgentId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }

    }

    public class CreateVisitRequestCommandHanlder : IRequestHandler<CreateVisitRequestCommand, Guid>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitRequestRepository _visitRequestRepository;
        public CreateVisitRequestCommandHanlder(IPropertyRepository propertyRepository, IVisitRequestRepository visitRequestRepository)
        {
            _propertyRepository=propertyRepository;
            _visitRequestRepository=visitRequestRepository;
        }

        public async Task<Guid> Handle(CreateVisitRequestCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property==null) throw new Exception("Property Not Found");

            var visitRequest = new VisitRequest
            {
                Id = Guid.NewGuid(),
                PropertyId=property.Id,
                PropertyName=property.Title,
                AgentId=property.AgentId,
                UserName=request.UserName,
                UserPhone=request.UserPhone,
                UserEmail=request.UserEmail,
                RequestDate=DateTime.UtcNow,
                Status="Pending"

            };

            await _visitRequestRepository.AddAsync(visitRequest, cancellationToken);
            return visitRequest.Id;




        }
    }
}
