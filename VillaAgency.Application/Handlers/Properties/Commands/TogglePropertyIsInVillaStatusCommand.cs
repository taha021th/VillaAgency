using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record TogglePropertyIsInVillaStatusCommand : IRequest
    {
        public Guid PropertyId { get; set; }
    }
    public class TogglePropertyIsInVillaStatusCommandHandler : IRequestHandler<TogglePropertyIsInVillaStatusCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private const int MaxVillaCategoryProperties = 1;
        public TogglePropertyIsInVillaStatusCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository= propertyRepository;
        }
        public async Task Handle(TogglePropertyIsInVillaStatusCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
            if (property is null)
            {
                throw new Exception("ملک مورد نظر یافت نشد.");
            }
            if (!property.IsInVilla)
            {
                var currentIsInVillaCount = await _propertyRepository.CountAsync(p => p.IsInVilla);
                if (currentIsInVillaCount>=MaxVillaCategoryProperties)
                {
                    throw new Exception($"امکان افزودن بیش از {MaxVillaCategoryProperties} ملک به دسته بندی ویلا برای نمایش در صفحه اصلی وجود ندارد.");
                }
            }
            property.IsInVilla= !property.IsInVilla;
            await _propertyRepository.UpdateAsync(property, cancellationToken);
        }
    }
}
