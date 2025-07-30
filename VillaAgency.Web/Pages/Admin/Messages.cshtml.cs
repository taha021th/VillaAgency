using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.ContactMessages.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class MessagesModel : PageModel
    {
        private readonly IMediator _mediator;

        public MessagesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<ContactMessage> Messages { get; set; } = new List<ContactMessage>();

        public async Task OnGetAsync()
        {
            Messages = await _mediator.Send(new GetAllContactMessagesQuery());
        }
    }
}
