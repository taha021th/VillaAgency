using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record TogglePropertyIsInPentHouseStatusCommand : IRequest
    {
        public Guid PropertyId { get; set; }
    }
    public class TogglePropertyIsInPentHouseStatusCommandHandler : IRequestHandler<TogglePropertyIsInPentHouseStatusCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private const int MaxIsInPentHouse = 1;
        public TogglePropertyIsInPentHouseStatusCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository=propertyRepository;
        }
        public async Task Handle(TogglePropertyIsInPentHouseStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property is null)
            {
                throw new Exception("ملک مورد نظر یافت نشد.");
            }
            if (!property.IsInPentHouse)
            {
                var currentIsInPentHouseCount = await _propertyRepository.CountAsync(p => p.IsInPentHouse);
                if (currentIsInPentHouseCount >=1)
                {
                    throw new Exception($"امکان افزودن بیش از  {MaxIsInPentHouse} ملک به دسته بندی پنت هاوس وجود ندارد.");
                }
            }
            property.IsInPentHouse = !property.IsInPentHouse;
            await _propertyRepository.UpdateAsync(property, cancellationToken);
        }

    }
}
