using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Dashboard.Queries;
using VillaAgency.Application.Handlers.ViewModels;

namespace VillaAgency.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public DashboardViewModel DashboardVm { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PeriodInDays { get; set; } = 7;

        public async Task<IActionResult> OnGetAsync()
        {

            if (PeriodInDays < 1)
            {
                PeriodInDays = 7;
            }


            DashboardVm = await _mediator.Send(new GetDashboardStatsQuery { PeriodInDays = this.PeriodInDays });

            if (DashboardVm == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
