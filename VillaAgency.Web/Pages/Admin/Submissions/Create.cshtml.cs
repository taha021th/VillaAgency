using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.PropertySubmissions.Queries;

namespace VillaAgency.Web.Pages.Admin.Submissions
{
    [Authorize(Roles = "Admin,Agent")]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoStorageService _videoStorageService;
        private readonly ICacheService _cacheService;

        public CreateModel(IMediator mediator, IFileStorageService fileStorageService, IVideoStorageService videoStorageService, ICacheService cacheService)
        {
            _mediator = mediator;
            _fileStorageService = fileStorageService;
            _videoStorageService = videoStorageService;
            _cacheService = cacheService;
        }

        [BindProperty]
        public CreatePropertyCommand CreatePropertyCommand { get; set; } = new();

        public SelectList CategoryList { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid SubmissionId { get; set; }

        public List<string> SubmittedImageUrls { get; set; } = new();
        public List<string> SubmittedVideoUrls { get; set; } = new();

        // پراپرتی جدید برای مدیریت خطا
        public bool IsPostBackWithError { get; set; } = false;

        // متد OnGetAsync بدون تغییر
        public async Task OnGetAsync()
        {
            await LoadCategoryList();

            if (SubmissionId != Guid.Empty)
            {
                var submission = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
                if (submission != null)
                {
                    CreatePropertyCommand.Title = submission.Title;
                    CreatePropertyCommand.Description = submission.Description;
                    CreatePropertyCommand.Address = submission.Address;
                    CreatePropertyCommand.Price = submission.Price;
                    CreatePropertyCommand.Area = submission.Area;
                    CreatePropertyCommand.Bedrooms = submission.Bedrooms;
                    CreatePropertyCommand.Floor = submission.Floor;
                    CreatePropertyCommand.FloorsCount = submission.FloorsCount;
                    CreatePropertyCommand.Unit = submission.Unit;
                    CreatePropertyCommand.UnitsCountInFloor = submission.UnitsCountInFloor;
                    CreatePropertyCommand.BuildDate = submission.BuildDate;
                    CreatePropertyCommand.CategoryId = submission.CategoryId;
                    CreatePropertyCommand.TransactionType = submission.TransactionType;
                    CreatePropertyCommand.FullName=submission.FullName;
                    CreatePropertyCommand.PhoneNumber=submission.PhoneNumber;

                    SubmittedImageUrls = submission.ImageUrls ?? new List<string>();
                    SubmittedVideoUrls = submission.VideoUrls ?? new List<string>();
                }
            }
        }

        // متد OnPostAsync اصلاح شده
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // به ویو اطلاع بده که با خطا برگشته‌ایم
                IsPostBackWithError = true;

                await LoadCategoryList();

                // نیاز به بارگذاری مجدد تصاویر و ویدئوها داریم تا در صفحه باقی بمانند
                var submissionOnErr = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
                if (submissionOnErr != null)
                {
                    // از لیست تصاویر و ویدئوهای ارسالی در فرم استفاده می‌کنیم چون ممکن است کاربر مواردی را حذف کرده باشد
                    SubmittedImageUrls = CreatePropertyCommand.ImageUrls?.ToList() ?? new List<string>();
                    SubmittedVideoUrls = CreatePropertyCommand.VideoUrls?.ToList() ?? new List<string>();
                }
                return Page();
            }

            // بقیه منطق OnPostAsync بدون تغییر باقی می‌ماند
            var originalSubmission = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
            if (originalSubmission != null)
            {
                var originalImageUrls = originalSubmission.ImageUrls ?? new List<string>();
                var submittedImageUrls = CreatePropertyCommand.ImageUrls ?? new List<string>();
                var imagesToDelete = originalImageUrls.Except(submittedImageUrls).ToList();
                foreach (var imageUrl in imagesToDelete)
                {
                    await _fileStorageService.DeleteFileAsync(Path.GetFileName(imageUrl), "images");
                }


                var originalVideoUrls = originalSubmission.VideoUrls ?? new List<string>();
                var submittedVideoUrls = CreatePropertyCommand.VideoUrls ?? new List<string>();
                var videosToDelete = originalVideoUrls.Except(submittedVideoUrls).ToList();
                foreach (var videoUrl in videosToDelete)
                {
                    await _videoStorageService.DeleteVideoAsync(Path.GetFileName(videoUrl), "videos");
                }
            }

            await _mediator.Send(CreatePropertyCommand);
            // ... حذف کش و تنظیم TempData
            await _cacheService.RemoveDataAsync("properties_first_page_list");
            TempData["success"] = "ملک با موفقیت از روی درخواست ثبت شد.";

            return RedirectToPage("/Admin/Properties/Index");
        }

        private async Task LoadCategoryList()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");
        }
    }
}