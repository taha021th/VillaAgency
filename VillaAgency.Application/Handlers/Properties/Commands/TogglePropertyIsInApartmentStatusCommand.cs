using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record TogglePropertyIsInApartmentStatusCommand : IRequest
    {
        public Guid PropertyId { get; set; }

    }
    public class TogglePropertyIsInApartmentStatusCommandHandler : IRequestHandler<TogglePropertyIsInApartmentStatusCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private const int MaxIsInApartment = 1;
        public TogglePropertyIsInApartmentStatusCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task Handle(TogglePropertyIsInApartmentStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property is null)
            {
                throw new Exception("ملک مورد نظر یافت نشد.");
            }
            if (!property.IsInApartment)
            {
                var currentIsInApartment = await _propertyRepository.CountAsync(p => p.IsInApartment);
                if (currentIsInApartment>=MaxIsInApartment)
                {
                    throw new Exception($"امکان افزودن بیش از  {MaxIsInApartment} ملک به دسته بندی آپارتمان وجود ندارد.");
                }
            }
            property.IsInApartment = !property.IsInApartment;
            await _propertyRepository.UpdateAsync(property, cancellationToken);
        }
    }
}
