using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.VisitRequests.Queries
{
    public record GetAllVisitRequestsQuery : IRequest<IEnumerable<VisitRequest>>
    {
    }

    public class GetAllVisitRequestQueryHandler : IRequestHandler<GetAllVisitRequestsQuery, IEnumerable<VisitRequest>>
    {
        private readonly IVisitRequestRepository _visitRequestRepository;
        public GetAllVisitRequestQueryHandler(IVisitRequestRepository visitRequestRepository)
        {
            _visitRequestRepository= visitRequestRepository;

        }

        public async Task<IEnumerable<VisitRequest>> Handle(GetAllVisitRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _visitRequestRepository.GetAllAsync(cancellationToken);
        }
    }
}
