using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.PropertySubmissions.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.Submissions
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        // با اختصاص یک لیست خالی به عنوان مقدار اولیه، وارنینگ برطرف می‌شود.
        public IEnumerable<PropertySubmission> PendingSubmissions { get; set; } = new List<PropertySubmission>();

        public async Task OnGetAsync()
        {
            // فقط درخواست‌هایی که در وضعیت "در انتظار بررسی" هستند را دریافت می‌کنیم
            PendingSubmissions = await _mediator.Send(new GetAllPropertySubmissionsQuery { Status = "Pending" });
        }
    }
}
