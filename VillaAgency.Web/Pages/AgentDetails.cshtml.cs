using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Application.Handlers.Users.Queries;
using VillaAgency.Domain.Entities;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages
{
    public class AgentDetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public AgentDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ApplicationUser Agent { get; set; }
        public IEnumerable<Property> Properties { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            // دریافت همزمان اطلاعات مشاور و املاک او
            var agentTask = _mediator.Send(new GetUserByIdQuery { Id = id });
            var propertiesTask = _mediator.Send(new GetPropertiesByAgentIdQuery { AgentId = id });

            await Task.WhenAll(agentTask, propertiesTask);

            Agent = await agentTask;
            Properties = await propertiesTask;

            if (Agent == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
