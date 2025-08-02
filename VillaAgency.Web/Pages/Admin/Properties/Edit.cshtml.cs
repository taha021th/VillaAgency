using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.Properties.Queries;

namespace VillaAgency.Web.Pages.Admin.Properties
{
    [Authorize(Roles = "Admin,Agent")]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoStorageService _videoStorageService;
        private readonly ICacheService _cacheService;

        public EditModel(IMediator mediator, IFileStorageService fileStorageService, IVideoStorageService videoStorageService, ICacheService cacheService)
        {
            _mediator = mediator;
            _fileStorageService = fileStorageService;
            _videoStorageService = videoStorageService;
            _cacheService=cacheService;
        }

        [BindProperty]
        public UpdatePropertyCommand PropertyCommand { get; set; } = new();

        [BindProperty]
        public List<IFormFile>? NewImageFiles { get; set; }
        [BindProperty]
        public List<IFormFile>? NewVideoFiles { get; set; }

        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            await LoadCategories();
            var property = await _mediator.Send(new GetPropertyByIdQuery(id));
            if (property == null)
            {
                return NotFound();
            }

            PropertyCommand = new UpdatePropertyCommand
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Floor = property.Floor,
                FloorsCount = property.FloorsCount,
                UnitsCountInFloor = property.UnitsCountInFloor,
                Unit = property.Unit,
                BuildDate = property.BuildDate,
                Address = property.Address,
                CategoryId = property.CategoryId,
                ImageUrls = property.ImageUrls ?? new List<string>(),
                VideoUrls = property.VideoUrls ?? new List<string>(),
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return Page();
            }

            // ۱. واکشی اطلاعات فعلی ملک برای یافتن فایل‌های حذف شده
            var originalProperty = await _mediator.Send(new GetPropertyByIdQuery(PropertyCommand.Id));
            if (originalProperty == null)
            {
                return NotFound();
            }

            // ۲. شناسایی و حذف تصاویر حذف شده از روی سرور
            var originalImageUrls = originalProperty.ImageUrls ?? new List<string>();
            var submittedImageUrls = PropertyCommand.ImageUrls ?? new List<string>();
            var imagesToDelete = originalImageUrls.Except(submittedImageUrls).ToList();

            foreach (var imageUrl in imagesToDelete)
            {
                var fileName = Path.GetFileName(imageUrl);
                // فرض بر این است که متد DeleteFileAsync در سرویس شما وجود دارد
                await _fileStorageService.DeleteFileAsync(fileName, "images");
            }

            // ۳. شناسایی و حذف ویدئوهای حذف شده از روی سرور
            var originalVideoUrls = originalProperty.VideoUrls ?? new List<string>();
            var submittedVideoUrls = PropertyCommand.VideoUrls ?? new List<string>();
            var videosToDelete = originalVideoUrls.Except(submittedVideoUrls).ToList();

            foreach (var videoUrl in videosToDelete)
            {
                var fileName = Path.GetFileName(videoUrl);
                // فرض بر این است که متد DeleteVideoAsync در سرویس شما وجود دارد
                await _videoStorageService.DeleteVideoAsync(fileName, "videos");
            }

            // ۴. آپلود تصاویر جدید و اضافه کردن آدرس آن‌ها به لیست
            if (NewImageFiles != null && NewImageFiles.Any())
            {
                var newImageUrls = await _fileStorageService.SaveFilesAsync(NewImageFiles, "images");
                submittedImageUrls.AddRange(newImageUrls);
            }

            // ۵. آپلود ویدئوهای جدید و اضافه کردن آدرس آن‌ها به لیست
            if (NewVideoFiles != null && NewVideoFiles.Any())
            {
                var newVideoUrls = await _videoStorageService.SaveVideosAsync(NewVideoFiles, "videos");
                submittedVideoUrls.AddRange(newVideoUrls);
            }

            // ۶. به‌روزرسانی لیست نهایی آدرس‌ها در Command
            PropertyCommand.ImageUrls = submittedImageUrls;
            PropertyCommand.VideoUrls = submittedVideoUrls;

            // ۷. ارسال دستور نهایی برای آپدیت در دیتابیس
            await _mediator.Send(PropertyCommand);
            await _cacheService.RemoveDataAsync("properties_first_page_list");

            return RedirectToPage("./Index");
        }

        private async Task LoadCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");
        }
    }
}