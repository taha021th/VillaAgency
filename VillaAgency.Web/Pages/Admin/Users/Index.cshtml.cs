using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Users.Queries;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public IEnumerable<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public Dictionary<Guid, IList<string>> UserRoles { get; set; } = new Dictionary<Guid, IList<string>>();

        public async Task OnGetAsync()
        {

            Users=await _mediator.Send(new GetAllUsersQuery());
            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id]=roles;
            }
        }
    }
}
