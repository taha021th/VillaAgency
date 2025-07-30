
using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.ContactMessages.Queries
{
    public class GetAllContactMessagesQuery : IRequest<IEnumerable<ContactMessage>>
    {
    }

    public class GetAllContactmessagesQueryHandler : IRequestHandler<GetAllContactMessagesQuery, IEnumerable<ContactMessage>>
    {
        private readonly IContactMessageRepository _contactMessageRepository;

        public GetAllContactmessagesQueryHandler(IContactMessageRepository contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }

        public async Task<IEnumerable<ContactMessage>> Handle(GetAllContactMessagesQuery request, CancellationToken cancellationToken)
        {
            await _contactMessageRepository.MarkAllAsReadAsync();
            return await _contactMessageRepository.GetAllAsync();
        }
    }
}
