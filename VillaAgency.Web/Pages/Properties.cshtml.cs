using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages
{
    // یک کلاس داخلی کوچک برای نگهداری داده‌های مورد نیاز در کش
    // این کلاس دقیقاً ساختار خروجی کوئری شما را شبیه‌سازی می‌کند
    public class PropertiesCacheModel
    {
        public IEnumerable<Property> Properties { get; set; }
        public int TotalCount { get; set; }
    }

    public class PropertiesModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private const int PageSize = 6;

        public PropertiesModel(IMediator mediator, ICacheService cacheService)
        {
            _mediator = mediator;
            _cacheService = cacheService;
        }

        // پراپرتی‌های فیلتر
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

        // پراپرتی‌های صفحه‌بندی
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

            bool hasFilters = !string.IsNullOrEmpty(SearchTerm) || CategoryId.HasValue ||
                              MinPrice.HasValue || MaxPrice.HasValue ||
                              MinArea.HasValue || MaxArea.HasValue;

            if (hasFilters || CurrentPage != 1)
            {
                await FetchFromDatabase();
                return;
            }

            const string cacheKey = "properties_first_page_list";
            var cachedResult = await _cacheService.GetDataAsync<PropertiesCacheModel>(cacheKey);

            if (cachedResult != null)
            {
                Properties = cachedResult.Properties;
                TotalPages = (int)Math.Ceiling(cachedResult.TotalCount / (double)PageSize);
            }
            else
            {
                // اینجا دیگر نیازی به ذخیره خروجی FetchFromDatabase نداریم
                // چون خود متد، پراپرتی‌های کلاس را مستقیماً پر می‌کند
                var resultFromDb = await FetchFromDatabase();

                if (resultFromDb.Properties.Any())
                {
                    var cacheData = new PropertiesCacheModel
                    {
                        Properties = resultFromDb.Properties,
                        TotalCount = resultFromDb.TotalCount
                    };
                    await _cacheService.SetDataAsync(cacheKey, cacheData, TimeSpan.FromMinutes(10));
                }
            }
        }

        private async Task LoadCategories()
        {
            const string categoriesCacheKey = "categories_list";
            var categories = await _cacheService.GetDataAsync<IEnumerable<Category>>(categoriesCacheKey);

            if (categories is null)
            {
                categories = await _mediator.Send(new GetAllCategoriesQuery());
                await _cacheService.SetDataAsync(categoriesCacheKey, categories, TimeSpan.FromHours(1));
            }

            CategoryList = new SelectList(categories, "Id", "Name", CategoryId);
        }

        // نوع خروجی متد را به Tuple صحیح تغییر می‌دهیم
        private async Task<(IEnumerable<Property> Properties, int TotalCount)> FetchFromDatabase()
        {
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

            var result = await _mediator.Send(query);

            // اطمینان حاصل می‌کنیم که result null نیست
            if (result.Properties != null)
            {
                Properties = result.Properties;
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)PageSize);
            }

            return result;
        }
    }
}