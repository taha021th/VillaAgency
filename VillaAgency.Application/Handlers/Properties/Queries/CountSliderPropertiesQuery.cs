using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record CountSliderPropertiesQuery : IRequest<int>
    {
    }
    public class CountSliderPropertiesQueryHanlder : IRequestHandler<CountSliderPropertiesQuery, int>
    {
        private readonly IPropertyRepository _propertyRepository;
        public CountSliderPropertiesQueryHanlder(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<int> Handle(CountSliderPropertiesQuery request, CancellationToken cancellationToken)
        {
            var count = await _propertyRepository.CountAsync(p => p.IsInSlider);
            return (int)count;
        }
    }
}
