using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertyRequests.Commands
{
    public record AddReportToPropertyRequestCommand : IRequest
    {
        public Guid RequestId { get; init; }
        public string Content { get; init; }
        public string? AgentName { get; init; }
    }
    public class AddReportToPropertyRequestCommandHandler : IRequestHandler<AddReportToPropertyRequestCommand>
    {
        private readonly IPropertyRequestRepository _propertyRequestRepository;
        public AddReportToPropertyRequestCommandHandler(IPropertyRequestRepository propertyRequestRepository)
        {
            _propertyRequestRepository = propertyRequestRepository;
        }

        public async Task Handle(AddReportToPropertyRequestCommand request, CancellationToken cancellationToken)
        {
            var propertyRequest = await _propertyRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);
            if (propertyRequest is null)
            {
                throw new Exception("Property request not found.");
            }
            var newRport = new PropertyRequestReport
            {
                Content=request.Content,
                ReportDate=DateTime.UtcNow,
                AgentName=request.AgentName,
            };
            propertyRequest.Reports.Add(newRport);
            await _propertyRequestRepository.UpdateAsync(propertyRequest.Id, propertyRequest, cancellationToken);
        }
    }
}
