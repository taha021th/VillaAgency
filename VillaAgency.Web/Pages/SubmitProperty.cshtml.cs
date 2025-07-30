using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.PropertySubmissions.Commands;

namespace VillaAgency.Web.Pages
{
    public class SubmitPropertyModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoStorageService _videoStorageService;

        public SubmitPropertyModel(IMediator mediator, IWebHostEnvironment webHostEnvironment, IFileStorageService fileStorageService, IVideoStorageService videoStorageService)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _fileStorageService = fileStorageService;
            _videoStorageService = videoStorageService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList CategoryList { get; set; }

        public class InputModel
        {

            [Required] public string FullName { get; set; }
            [Required] public string PhoneNumber { get; set; }
            [Required] public string TransactionType { get; set; }
            [Required] public string Title { get; set; }
            [Required] public string Description { get; set; }
            [Required] public string Address { get; set; }
            [Required] public decimal Price { get; set; }
            [Required] public int Area { get; set; }
            [Required] public int Bedrooms { get; set; }
            [Required] public int Floor { get; set; }
            [Required] public int FloorsCount { get; set; }
            [Required] public int Unit { get; set; }
            [Required] public int UnitsCountInFloor { get; set; }
            [Required] public string BuildDate { get; set; } = string.Empty;
            [Required] public Guid CategoryId { get; set; }
            public List<IFormFile>? ImageFiles { get; set; }
            public List<IFormFile>? VideoFiles { get; set; }
        }

        public async Task OnGetAsync()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery());
                CategoryList = new SelectList(categories, "Id", "Name");
                return Page();
            }



            var command = new CreatePropertySubmissionCommand
            {
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber,
                TransactionType = Input.TransactionType,
                Title = Input.Title,
                Description = Input.Description,
                Address = Input.Address,
                Price = Input.Price,
                Area = Input.Area,
                Bedrooms = Input.Bedrooms,
                Floor = Input.Floor,
                FloorsCount = Input.FloorsCount,
                Unit = Input.Unit,
                UnitsCountInFloor = Input.UnitsCountInFloor,
                BuildDate = Input.BuildDate,
                CategoryId = Input.CategoryId
            };

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (Input.ImageFiles !=null && Input.ImageFiles.Any())
            {
                var getUrls = await _fileStorageService.SaveFilesAsync(Input.ImageFiles, "images");
                command.ImageUrls=getUrls;
            }
            // **منطق جدید:** آپلود چندین فایل ویدئو و ایجاد URL برای هر کدام
            if (Input.VideoFiles != null && Input.VideoFiles.Any())
            {
                var getUrls = await _videoStorageService.SaveVideosAsync(Input.VideoFiles, "videos");
                command.VideoUrls=getUrls;
            }


            await _mediator.Send(command);

            TempData["SuccessMessage"] = "ملک شما با موفقیت ثبت شد. کارشناسان ما به زودی با شما تماس خواهند گرفت.";
            return RedirectToPage();
        }
    }
}
