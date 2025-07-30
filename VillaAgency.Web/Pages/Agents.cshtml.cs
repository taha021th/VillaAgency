using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Users.Queries;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages
{
    public class AgentsModel : PageModel
    {
        private readonly IMediator _mediator;
        public AgentsModel(IMediator mediator)
        {
            _mediator=mediator;
        }

        public IEnumerable<ApplicationUser> Agents { get; set; } = new List<ApplicationUser>();
        public async Task OnGetAsync()
        {
            Agents = await _mediator.Send(new GetAllAgentsQuery());
        }
    }
}
