using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Categories.Commands;
using VillaAgency.Application.Handlers.Categories.Queries;

namespace VillaAgency.Web.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        public EditModel(IMediator mediator)
        {
            _mediator=mediator;
        }
        [BindProperty]
        public UpdateCategoryCommand Command { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {

            var category = await _mediator.Send(new GetCategoryByIdQuery { Id=Id });
            if (category==null) return NotFound();

            Command.Id=category.Id;
            Command.Name=category.Name;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid) return Page();
            await _mediator.Send(Command);
            return RedirectToPage("./Index");
        }
    }
}
