using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record DeletePropertyCommand(Guid id) : IRequest;


    public class PropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        public PropertyCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            await _propertyRepository.DeleteAsync(request.id, cancellationToken);
        }
    }


}
