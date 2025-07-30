using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.VisitRequests.Commands
{
    public record AddReportToVisitRequestCommand : IRequest
    {
        public Guid VisitRequestId { get; set; }
        public string Notes { get; set; }
    }

    public class AddReportToVisitRequestCommandHandler : IRequestHandler<AddReportToVisitRequestCommand>
    {

        private readonly IVisitRequestRepository _visitRequestRepository;
        public AddReportToVisitRequestCommandHandler(IVisitRequestRepository visitRequestRepository)
        {
            _visitRequestRepository = visitRequestRepository;
        }

        public async Task Handle(AddReportToVisitRequestCommand request, CancellationToken cancellationToken)
        {
            var visitRequest = await _visitRequestRepository.GetByIdAsync(request.VisitRequestId, cancellationToken);
            if (visitRequest==null)
            {
                throw new Exception("Visit request not found.");
            }
            if (visitRequest.Reports == null)
            {
                visitRequest.Reports = new List<AgentReport>();
            }

            var newReport = new AgentReport
            {
                Id = Guid.NewGuid(),
                Notes = request.Notes,
                ReportDate=DateTime.UtcNow

            };
            visitRequest.Reports.Add(newReport);
            visitRequest.Status="Contacted";
            await _visitRequestRepository.UpdateAsync(visitRequest, cancellationToken);
        }
    }
}
