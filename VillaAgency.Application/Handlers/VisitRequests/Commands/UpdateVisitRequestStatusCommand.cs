using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.VisitRequests.Commands
{
    public record UpdateVisitRequestStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        public string NewStatus { get; set; }
    }
    public class UpdateVisitRequestStatusCommandHandler : IRequestHandler<UpdateVisitRequestStatusCommand>
    {
        private readonly IVisitRequestRepository _visitRequestRepository;
        public UpdateVisitRequestStatusCommandHandler(IVisitRequestRepository visitRequestRepository)
        {
            _visitRequestRepository = visitRequestRepository;
        }

        public async Task Handle(UpdateVisitRequestStatusCommand request, CancellationToken cancellationToken)
        {
            var visitRequeste = await _visitRequestRepository.GetByIdAsync(request.Id, cancellationToken);
            if (visitRequeste==null)
            {
                throw new Exception("Visit request not found");
            }
            visitRequeste.Status=request.NewStatus;
            await _visitRequestRepository.UpdateAsync(visitRequeste, cancellationToken);
        }
    }
}
