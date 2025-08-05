using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record CountIsFeaturePropertiesQuery : IRequest<int>
    {
    }
    public class CountIsFeaturePropertiesQueryHandler : IRequestHandler<CountIsFeaturePropertiesQuery, int>
    {

        private readonly IPropertyRepository _propertyRepository;
        public CountIsFeaturePropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository=propertyRepository;
        }

        public async Task<int> Handle(CountIsFeaturePropertiesQuery request, CancellationToken cancellationToken)
        {
            var count = await _propertyRepository.CountAsync(p => p.IsFeatured);
            return (int)count;
        }
    }
}
