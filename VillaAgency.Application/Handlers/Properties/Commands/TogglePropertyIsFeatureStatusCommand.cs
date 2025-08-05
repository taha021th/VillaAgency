using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record TogglePropertyIsFeatureStatusCommand : IRequest
    {
        public Guid PropertyId { get; set; }
    }
    public class TogglePropertyIsFeatureStatusCommandHandler : IRequestHandler<TogglePropertyIsFeatureStatusCommand>
    {

        private readonly IPropertyRepository _propertyRepository;
        private const int MaxIsFeatureProperties = 1;
        public TogglePropertyIsFeatureStatusCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository=propertyRepository;
        }

        public async Task Handle(TogglePropertyIsFeatureStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property is null)
            {
                throw new Exception("ملک مورد نظر یافت نشد.");

            }
            if (!property.IsFeatured)
            {

                var currentIsFeatureCount = await _propertyRepository.CountAsync(p => p.IsFeatured);
                if (currentIsFeatureCount >= MaxIsFeatureProperties)
                {
                    throw new Exception($"امکان افزودن بیش از {MaxIsFeatureProperties} ملک ویزه وجود ندارد.");
                }
            }
            property.IsFeatured = !property.IsFeatured;
            await _propertyRepository.UpdateAsync(property, cancellationToken);
        }
    }
}
