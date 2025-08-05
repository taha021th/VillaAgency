using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Handlers.Properties.Commands;
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
        public int TotalCount { get; set; }

        // **پراپرتی جدید برای نگهداری تعداد املاک در اسلایدر**
        public int SliderPropertiesCount { get; set; }
        public int IsInVillaPropertiesCount { get; set; }
        public int IsInApartmentPropertiesCount { get; set; }
        public int IsInPentHousePropertiesCount { get; set; }
        public int IsFeaturePropertiesCount { get; set; }

        public async Task OnGetAsync()
        {
            var (properties, totalCount) = await _mediator.Send(new GetPaginatedAdminPropertiesQuery { PageNum = this.PageNum, PageSize = PageSize });

            Properties = properties;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            // شمارش تعداد املاک موجود در اسلایدر
            SliderPropertiesCount = await _mediator.Send(new CountSliderPropertiesQuery());
            IsInVillaPropertiesCount = await _mediator.Send(new CountIsInVillaPropertiesQuery());
            IsInApartmentPropertiesCount = await _mediator.Send(new CountIsInApartmentPropertiesQuery());
            IsInPentHousePropertiesCount = await _mediator.Send(new CountIsInApartmentPropertiesQuery());
            IsFeaturePropertiesCount = await _mediator.Send(new CountIsFeaturePropertiesQuery());
        }

        /// <summary>
        /// این Handler با کلیک روی دکمه‌های "افزودن/حذف از اسلایدر" فراخوانی می‌شود
        /// </summary>
        public async Task<IActionResult> OnPostToggleSliderStatusAsync(Guid id)
        {
            try
            {
                await _mediator.Send(new TogglePropertySliderStatusCommand { PropertyId = id });
                TempData["success"] = "وضعیت اسلایدر ملک با موفقیت تغییر کرد.";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            // بازگشت به همان صفحه‌ای که کاربر در آن قرار داشت
            return RedirectToPage(new { pageNum = PageNum });
        }
        public async Task<IActionResult> OnPostTogglePropertyIsInVillaStatusAsync(Guid id)
        {
            try
            {
                await _mediator.Send(new TogglePropertyIsInVillaStatusCommand { PropertyId = id });
                TempData["success"]= "ملک به دسته بندی ویلا در صفحه اصلی اضافه شد.";
            }
            catch (Exception ex)
            {
                TempData["error"]=ex.Message;
            }
            return RedirectToPage(new { pageNum = PageNum });
        }
        public async Task<IActionResult> OnPostTogglePropertyIsInPentHouseStatusAsync(Guid id)
        {
            try
            {
                await _mediator.Send(new TogglePropertyIsInPentHouseStatusCommand { PropertyId = id });
                TempData["success"]= "ملک به دسته بندی ویلا در صفحه اصلی اضافه شد.";
            }
            catch (Exception ex)
            {
                TempData["error"]=ex.Message;
            }
            return RedirectToPage(new { pageNum = PageNum });
        }
        public async Task<IActionResult> OnPostTogglePropertyIsInApartmentStatusAsync(Guid id)
        {
            try
            {
                await _mediator.Send(new TogglePropertyIsInApartmentStatusCommand { PropertyId = id });
                TempData["success"]= "ملک به دسته بندی ویلا در صفحه اصلی اضافه شد.";
            }
            catch (Exception ex)
            {
                TempData["error"]=ex.Message;
            }
            return RedirectToPage(new { pageNum = PageNum });
        }
        public async Task<IActionResult> OnPostToggleIsFeaturePropertyStatusAsync(Guid id)
        {

            try
            {
                await _mediator.Send(new TogglePropertyIsFeatureStatusCommand { PropertyId = id });
                TempData["success"] ="ملک به ملک ویژه اضافه شد.";
            }
            catch (Exception ex)
            {

                TempData["error"]=ex.Message;

            }
            return RedirectToPage(new { PageNum = PageNum });
        }
    }
}
