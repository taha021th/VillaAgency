using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.VisitRequests.Commands;
using VillaAgency.Application.Handlers.VisitRequests.Queries;
using VillaAgency.Domain.Entities;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages.Admin
{
    [Authorize(Roles = "Admin,Agent")]
    public class VisitRequestsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public VisitRequestsModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        // --- شروع تغییر ---
        // پراپرتی را با یک لیست خالی مقداردهی اولیه می‌کنیم
        public IEnumerable<VisitRequest> Requests { get; set; } = new List<VisitRequest>();
        // --- پایان تغییر ---

        [BindProperty]
        public AddReportToVisitRequestCommand ReportCommand { get; set; }

        public async Task OnGetAsync()
        {
            if (User.IsInRole("Admin"))
            {
                Requests = await _mediator.Send(new GetAllVisitRequestsQuery());
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    Requests = await _mediator.Send(new GetVisitRequestsByAgentIdQuery { AgentId = currentUser.Id });
                }
            }
        }

        public async Task<IActionResult> OnPostAddReportAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }
            await _mediator.Send(ReportCommand);
            return RedirectToPage();
        }
    }
}
