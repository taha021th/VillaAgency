using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Common.Interfaces.Services; // 👈 اضافه شد
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.PropertySubmissions.Queries;

namespace VillaAgency.Web.Pages.Admin.Submissions
{
    [Authorize(Roles = "Admin,Agent")]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService; // 👈 اضافه شد
        private readonly IVideoStorageService _videoStorageService; // 👈 اضافه شد

        public CreateModel(IMediator mediator, IFileStorageService fileStorageService, IVideoStorageService videoStorageService)
        {
            _mediator = mediator;
            _fileStorageService = fileStorageService; // 👈 اضافه شد
            _videoStorageService = videoStorageService; // 👈 اضافه شد
        }

        [BindProperty]
        public CreatePropertyCommand CreatePropertyCommand { get; set; } = new();

        public SelectList CategoryList { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid SubmissionId { get; set; }

        public List<string> SubmittedImageUrls { get; set; } = new();
        public List<string> SubmittedVideoUrls { get; set; } = new();


        public async Task OnGetAsync()
        {
            await LoadCategoryList();

            if (SubmissionId != Guid.Empty)
            {
                var submission = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
                if (submission != null)
                {
                    // پر کردن فیلدهای متنی فرم
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

                    // پر کردن لیست URL ها برای نمایش در View
                    SubmittedImageUrls = submission.ImageUrls ?? new List<string>();
                    SubmittedVideoUrls = submission.VideoUrls ?? new List<string>();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryList();
                // اگر مدل نامعتبر بود، باید دوباره لیست عکس‌ها را برای نمایش پر کنیم
                var submissionOnErr = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
                if (submissionOnErr != null)
                {
                    SubmittedImageUrls = submissionOnErr.ImageUrls ?? new List<string>();
                    SubmittedVideoUrls = submissionOnErr.VideoUrls ?? new List<string>();
                }
                return Page();
            }

            // واکشی اطلاعات اصلی درخواست برای مقایسه و حذف فایل
            var originalSubmission = await _mediator.Send(new GetPropertySubmissionByIdQuery { Id = SubmissionId });
            if (originalSubmission != null)
            {
                // شناسایی و حذف تصاویر اضافی
                var originalImageUrls = originalSubmission.ImageUrls ?? new List<string>();
                var submittedImageUrls = CreatePropertyCommand.ImageUrls ?? new List<string>();
                var imagesToDelete = originalImageUrls.Except(submittedImageUrls).ToList();
                foreach (var imageUrl in imagesToDelete)
                {
                    await _fileStorageService.DeleteFileAsync(Path.GetFileName(imageUrl), "images");
                }

                // شناسایی و حذف ویدئوهای اضافی
                var originalVideoUrls = originalSubmission.VideoUrls ?? new List<string>();
                var submittedVideoUrls = CreatePropertyCommand.VideoUrls ?? new List<string>();
                var videosToDelete = originalVideoUrls.Except(submittedVideoUrls).ToList();
                foreach (var videoUrl in videosToDelete)
                {
                    await _videoStorageService.DeleteVideoAsync(Path.GetFileName(videoUrl), "videos");
                }
            }

            await _mediator.Send(CreatePropertyCommand);
            TempData["success"] = "ملک با موفقیت از روی درخواست ثبت شد.";

            // اینجا باید درخواست (Submission) را هم از دیتابیس حذف یا وضعیت آن را تغییر دهید
            // (این بخش به منطق برنامه شما بستگی دارد)

            return RedirectToPage("/Admin/Properties/Index");
        }

        private async Task LoadCategoryList()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");
        }
    }
}