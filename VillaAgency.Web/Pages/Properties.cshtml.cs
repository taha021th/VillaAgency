using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages
{
    public class PropertiesModel : PageModel
    {
        private readonly IMediator _mediator;
        private const int PageSize = 6;

        public PropertiesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        // پراپرتی‌های فیلتر (بدون تغییر)
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal? MinPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? MinArea { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? MaxArea { get; set; }

        // پراپرتی‌های صفحه‌بندی (بدون تغییر)
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public SelectList CategoryList { get; set; }
        public IEnumerable<Property> Properties { get; set; } = new List<Property>();

        public async Task OnGetAsync()
        {
            await LoadCategories();

            // ساخت کوئری جدید با تمام پارامترها
            var query = new GetPaginatedFilteredPropertiesQuery
            {
                SearchTerm = SearchTerm,
                CategoryId = CategoryId,
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                MinArea = MinArea,
                MaxArea = MaxArea,
                PageNumber = CurrentPage,
                PageSize = PageSize
            };

            // ارسال کوئری و دریافت نتیجه بهینه
            var result = await _mediator.Send(query);

            // اختصاص نتایج به پراپرتی‌های مدل
            Properties = result.Properties;
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)PageSize);
        }

        private async Task LoadCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name", CategoryId); // مقدار انتخاب شده را هم پاس می‌دهیم
        }
    }
}
