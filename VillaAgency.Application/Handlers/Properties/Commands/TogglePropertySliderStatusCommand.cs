using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record TogglePropertySliderStatusCommand : IRequest
    {
        public Guid PropertyId { get; init; }

    }
    public class TogglePropertySliderStatusCommandHandler : IRequestHandler<TogglePropertySliderStatusCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private const int MaxSliderProperties = 6;
        public TogglePropertySliderStatusCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository= propertyRepository;
        }
        public async Task Handle(TogglePropertySliderStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property is null)
            {
                throw new Exception("ملک مورد نظر یافت نشد.");
            }
            if (!property.IsInSlider)
            {
                var currentSliderCount = await _propertyRepository.CountAsync(p => p.IsInSlider);
                if (currentSliderCount >= MaxSliderProperties)
                {
                    throw new Exception($"امکان افزودن بیش از  {MaxSliderProperties} ملک به اسلایدر وجود ندارد.");
                }

            }
            property.IsInSlider= !property.IsInSlider;
            await _propertyRepository.UpdateAsync(property, cancellationToken);
        }
    }
}
