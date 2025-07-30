using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.VisitRequests.Queries
{
    public record GetVisitRequestsByAgentIdQuery : IRequest<IEnumerable<VisitRequest>>
    {
        public Guid AgentId { get; set; }
    }

    public class GetVisitRequestsByAgentIdQueryHandler : IRequestHandler<GetVisitRequestsByAgentIdQuery, IEnumerable<VisitRequest>>
    {
        private readonly IVisitRequestRepository _visitRequestRepository;
        public GetVisitRequestsByAgentIdQueryHandler(IVisitRequestRepository visitRequestRepository)
        {
            _visitRequestRepository = visitRequestRepository;
        }
        public async Task<IEnumerable<VisitRequest>> Handle(GetVisitRequestsByAgentIdQuery request, CancellationToken cancellationToken)
        {
            return await _visitRequestRepository.GetByAgentIdAsync(request.AgentId);
        }
    }
}
