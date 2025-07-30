using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertySubmissions.Queries
{
    public class GetPropertySubmissionByIdQuery : IRequest<PropertySubmission?>
    {
        public Guid Id { get; set; }
    }

    public class GetPropertySubmissionByIdQueryHandler : IRequestHandler<GetPropertySubmissionByIdQuery, PropertySubmission?>
    {
        private readonly IPropertySubmissionRepository _repository;

        public GetPropertySubmissionByIdQueryHandler(IPropertySubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PropertySubmission?> Handle(GetPropertySubmissionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
