using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record CountIsInVillaPropertiesQuery : IRequest<int>
    {
    }
    public class CountIsInVillaPropertiesQueryHandler : IRequestHandler<CountIsInVillaPropertiesQuery, int>
    {
        private readonly IPropertyRepository _propertyRepository;
        public CountIsInVillaPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository= propertyRepository;
        }
        public async Task<int> Handle(CountIsInVillaPropertiesQuery request, CancellationToken cancellationToken)
        {
            var count = await _propertyRepository.CountAsync(p => p.IsInVilla);
            return (int)count;
        }
    }
}
