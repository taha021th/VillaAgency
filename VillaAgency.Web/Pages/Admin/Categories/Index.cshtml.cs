using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator= mediator;
        }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public async Task OnGetAsync()
        {
            Categories = await _mediator.Send(new GetAllCategoriesQuery());
        }
    }
}
