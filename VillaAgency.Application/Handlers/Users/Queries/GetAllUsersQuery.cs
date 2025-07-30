using MediatR;
using Microsoft.AspNetCore.Identity;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Handlers.Users.Queries
{
    public record GetAllUsersQuery : IRequest<IEnumerable<ApplicationUser>>
    {
    }
    public class GetAllUsersQueryHanlder : IRequestHandler<GetAllUsersQuery, IEnumerable<ApplicationUser>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public GetAllUsersQueryHanlder(UserManager<ApplicationUser> usermanager)
        {
            _userManager = usermanager;
        }

        public async Task<IEnumerable<ApplicationUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();
            return await Task.FromResult(users);

        }
    }
}
