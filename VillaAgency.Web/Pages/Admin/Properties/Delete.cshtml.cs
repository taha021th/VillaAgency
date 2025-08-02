using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages.Admin.Properties
{
    [Authorize(Roles = "Admin,Agent")]
    public class DeleteModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public DeleteModel(IMediator mediator, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _cacheService=cacheService;
        }

        [BindProperty]
        public Property Property { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // دریافت اطلاعات ملک برای نمایش در صفحه
            var property = await _mediator.Send(new GetPropertyByIdQuery(id));
            if (property == null)
            {
                return NotFound();
            }
            Property = property;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // --- شروع تغییر ---
            // قبل از حذف از دیتابیس، باید اطلاعات ملک را دوباره بگیریم تا لیست تصاویر آن را داشته باشیم
            // ما از Property.Id که از فرم POST شده استفاده می‌کنیم
            var propertyToDelete = await _mediator.Send(new GetPropertyByIdQuery(Property.Id));
            if (propertyToDelete == null)
            {
                // اگر ملک پیدا نشد، احتمالا در یک تب دیگر حذف شده است
                return RedirectToPage("./Index");
            }

            // --- منطق حذف فایل‌های تصویر ---
            if (propertyToDelete.ImageUrls != null && propertyToDelete.ImageUrls.Any())
            {
                foreach (var imageName in propertyToDelete.ImageUrls)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            // ارسال Command به MediatR برای حذف ملک از دیتابیس
            await _mediator.Send(new DeletePropertyCommand(Property.Id));
            await _cacheService.RemoveDataAsync("properties_first_page_list");

            return RedirectToPage("./Index");
            // --- پایان تغییر ---
        }
    }
}