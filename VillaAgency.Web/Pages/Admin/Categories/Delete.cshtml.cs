using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Categories.Commands;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly IMediator _mediator;
        public DeleteModel(IMediator mediator)
        {
            _mediator= mediator;
        }
        [BindProperty]
        public DeleteCategoryCommand Command { get; set; } = new();
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery { Id=id });
            if (category==null) return NotFound();
            Category=category;
            Command.Id= category.Id;
            return Page();


        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(Command);
            return RedirectToPage("./Index");
        }

    }
}
