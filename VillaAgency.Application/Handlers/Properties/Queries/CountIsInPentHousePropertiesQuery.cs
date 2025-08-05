using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record CountIsInPentHousePropertiesQuery : IRequest<int>
    {
    }
    public class CountIsInPentHousePropertiesQueryHandler : IRequestHandler<CountIsInPentHousePropertiesQuery, int>
    {

        private readonly IPropertyRepository _propertyRepository;
        public CountIsInPentHousePropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository=propertyRepository;

        }

        public async Task<int> Handle(CountIsInPentHousePropertiesQuery request, CancellationToken cancellationToken)
        {
            var count = await _propertyRepository.CountAsync(p => p.IsInPentHouse);
            return (int)count;
        }
    }
}
