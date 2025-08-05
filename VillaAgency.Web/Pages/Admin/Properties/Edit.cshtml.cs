using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OpenApi.Extensions;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Domain.Entities.Enums;

namespace VillaAgency.Web.Pages.Admin.Properties
{
    [Authorize(Roles = "Admin,Agent")]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoStorageService _videoStorageService;

        public EditModel(IMediator mediator, IFileStorageService fileStorageService, IVideoStorageService videoStorageService)
        {
            _mediator = mediator;
            _fileStorageService = fileStorageService;
            _videoStorageService = videoStorageService;
        }

        [BindProperty]
        public UpdatePropertyCommand PropertyCommand { get; set; } = new();

        public SelectList CategoryList { get; set; }
        // **پراپرتی جدید برای لیست کشویی نوع ملک**
        public SelectList PropertyTypeList { get; set; }

        [BindProperty]
        public List<IFormFile>? NewImageFiles { get; set; }
        [BindProperty]
        public List<IFormFile>? NewVideoFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var property = await _mediator.Send(new GetPropertyByIdQuery(id));
            if (property == null)
            {
                return NotFound();
            }

            // Map entity to command
            PropertyCommand.Id = property.Id;
            PropertyCommand.Title = property.Title;
            PropertyCommand.Description = property.Description;
            PropertyCommand.Address = property.Address;
            PropertyCommand.Price = property.Price;
            PropertyCommand.Area = property.Area;
            PropertyCommand.Bedrooms = property.Bedrooms;
            PropertyCommand.Floor = property.Floor;
            PropertyCommand.FloorsCount = property.FloorsCount;
            PropertyCommand.Unit = property.Unit;
            PropertyCommand.UnitsCountInFloor = property.UnitsCountInFloor;
            PropertyCommand.BuildDate = property.BuildDate;
            PropertyCommand.CategoryId = property.CategoryId;
            PropertyCommand.ImageUrls = property.ImageUrls;
            PropertyCommand.VideoUrls = property.VideoUrls;
            PropertyCommand.FullName = property.FullName;
            PropertyCommand.PhoneNumber = property.PhoneNumber;
            PropertyCommand.TransactionType=property.TransactionType;

            await LoadPrerequisites();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadPrerequisites();
                return Page();
            }

            if (NewImageFiles != null && NewImageFiles.Any())
            {
                var newImageUrls = await _fileStorageService.SaveFilesAsync(NewImageFiles, "images/properties");
                if (PropertyCommand.ImageUrls == null) PropertyCommand.ImageUrls = new List<string>();
                PropertyCommand.ImageUrls.AddRange(newImageUrls);
            }

            if (NewVideoFiles != null && NewVideoFiles.Any())
            {
                var newVideoUrls = await _videoStorageService.SaveVideosAsync(NewVideoFiles, "videos/properties");
                if (PropertyCommand.VideoUrls == null) PropertyCommand.VideoUrls = new List<string>();
                PropertyCommand.VideoUrls.AddRange(newVideoUrls);
            }

            await _mediator.Send(PropertyCommand);
            TempData["success"] = "ملک با موفقیت ویرایش شد.";
            return RedirectToPage("./Index");
        }

        private async Task LoadPrerequisites()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");

            // ساخت SelectList با نام‌های فارسی برای Enum
            PropertyTypeList = new SelectList(
                Enum.GetValues(typeof(PropertyType)).Cast<Enum>()
                    .Select(e => new { Value = e, DisplayName = e.GetDisplayName() }),
                "Value",
                "DisplayName"
            );
        }
    }
}
