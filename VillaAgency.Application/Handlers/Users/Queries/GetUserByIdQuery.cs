using MediatR;
using Microsoft.AspNetCore.Identity;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Handlers.Users.Queries
{
    public class GetUserByIdQuery : IRequest<ApplicationUser>
    {
        public Guid Id { get; set; }
    }
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByIdAsync(request.Id.ToString());
        }
    }
}
