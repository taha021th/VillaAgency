using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record CountIsInApartmentPropertiesQuery : IRequest<int>
    {
    }
    public class CountIsInApartmentPropertiesQueryHandler : IRequestHandler<CountIsInApartmentPropertiesQuery, int>
    {

        private readonly IPropertyRepository _propertyRepository;
        public CountIsInApartmentPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository=propertyRepository;
        }

        public async Task<int> Handle(CountIsInApartmentPropertiesQuery request, CancellationToken cancellationToken)
        {
            var count = await _propertyRepository.CountAsync(p => p.IsInApartment);
            return (int)count;
        }
    }
}
