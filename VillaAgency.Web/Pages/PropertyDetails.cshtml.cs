using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Application.Handlers.VisitRequests.Commands;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages
{
    public class PropertyDetailsModel : PageModel
    {
        private readonly IMediator _mediator;

        public PropertyDetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Property Property { get; set; }
        public string BuildDate { get; set; }


        [BindProperty]
        public CreateVisitRequestCommand VisitRequestCommand { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            // دریافت داده‌های ملک در سمت سرور
            Property = await _mediator.Send(new GetPropertyByIdQuery(id));

            BuildDate=Property.BuildDate;


            if (Property == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(VisitRequestCommand.PropertyId);
                return Page();
            }
            await _mediator.Send(VisitRequestCommand);
            return RedirectToPage(new { id = VisitRequestCommand.PropertyId });

        }
    }

}
