using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.Properties
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
        public IEnumerable<Property> Properties { get; set; } = new List<Property>();

        [BindProperty(SupportsGet = true)]
        public int PageNum { get; set; } = 1;
        public int TotalPages { get; set; }


        public async Task OnGetAsync()
        {

            var result = await _mediator.Send(new GetPaginatedAdminPropertiesQuery { PageNum=this.PageNum, PageSize=PageSize });
            Properties = result.properties;
            TotalPages=(int)Math.Ceiling(result.TotalCount/(double)PageSize);
        }
    }
}