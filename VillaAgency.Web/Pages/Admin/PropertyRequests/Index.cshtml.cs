using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.PropertyRequests.Commands;
using VillaAgency.Application.Handlers.PropertyRequests.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.PropertyRequests
{
    [Authorize(Roles = "Admin,Agent")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private const int PageSize = 10;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<PropertyRequest> PropertyRequests { get; set; } = new List<PropertyRequest>();

        [BindProperty(SupportsGet = true)]
        public int PageNum { get; set; } = 1;

        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        [BindProperty]
        public AddReportToPropertyRequestCommand AddReportCommand { get; set; }

        public async Task OnGetAsync()
        {
            if (PageNum < 1)
            {
                PageNum = 1;
            }

            var (requests, totalCount) = await _mediator.Send(new GetPaginatedPropertyRequestsQuery { PageNum = this.PageNum, PageSize = PageSize });

            PropertyRequests = requests;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // در صورت خطا، صفحه را با داده‌های فعلی دوباره بارگذاری می‌کنیم
                await OnGetAsync();
                return Page();
            }

            var command = AddReportCommand with { AgentName = User.Identity?.Name ?? "Unknown Agent" };
            await _mediator.Send(command);

            TempData["SuccessMessage"] = "گزارش با موفقیت ثبت شد.";
            return RedirectToPage();
        }
    }
}