using MediatR;
using Microsoft.AspNetCore.Identity;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Handlers.Users.Queries
{
    public record GetAllAgentsQuery : IRequest<IEnumerable<ApplicationUser>>
    {
    }

    public class GetAllAgentsQueryHanlder : IRequestHandler<GetAllAgentsQuery, IEnumerable<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllAgentsQueryHanlder(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            return agents;
        }
    }
}
