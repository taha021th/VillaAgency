

using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertySubmissions.Queries
{
    public class GetAllPropertySubmissionsQuery : IRequest<IEnumerable<PropertySubmission>>
    {
        public string? Status { get; set; }
    }
    public class GetAllPropertySubmissionsQueryHandler : IRequestHandler<GetAllPropertySubmissionsQuery, IEnumerable<PropertySubmission>>
    {
        private readonly IPropertySubmissionRepository _propertySubmissionRepository;
        public GetAllPropertySubmissionsQueryHandler(IPropertySubmissionRepository propertySubmissionRepository)
        {
            _propertySubmissionRepository = propertySubmissionRepository;
        }
        public async Task<IEnumerable<PropertySubmission>> Handle(GetAllPropertySubmissionsQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Status))
            {
                return await _propertySubmissionRepository.FindAsync(s => s.Status==request.Status);
            }
            return await _propertySubmissionRepository.GetAllAsync(cancellationToken);
        }
    }
}
