using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.Properties.Commands;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages.Admin.Properties
{
    [Authorize(Roles = "Admin,Agent")]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoStorageService _videoStorageService;
        public CreateModel(IMediator mediator, UserManager<ApplicationUser> userManager, IFileStorageService fileStorageService, IVideoStorageService videoStorageService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _videoStorageService = videoStorageService;
        }

        [BindProperty]
        public CreatePropertyCommand PropertyCommand { get; set; } = new();
        public SelectList CategoryList { get; set; }

        [BindProperty]
        public List<IFormFile>? ImageFiles { get; set; }
        [BindProperty]
        public List<IFormFile>? VideoFiles { get; set; }

        public async Task OnGetAsync()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = new SelectList(categories, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            if (!string.IsNullOrWhiteSpace(PropertyCommand.BuildDate))
            {
                PropertyCommand.BuildDate=PropertyCommand.BuildDate;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            PropertyCommand.AgentId = currentUser.Id;
            PropertyCommand.AgentName = currentUser.FullName;

            if (ImageFiles != null && ImageFiles.Any())
            {
                var getUrl = await _fileStorageService.SaveFilesAsync(ImageFiles, "images");
                PropertyCommand.ImageUrls = getUrl;
            }

            else
            {
                PropertyCommand.ImageUrls = new List<string>();
            }

            if (VideoFiles != null && VideoFiles.Any())
            {
                var getUrl = await _videoStorageService.SaveVideosAsync(VideoFiles, "videos");
                PropertyCommand.VideoUrls = getUrl;
            }
            else
            {
                PropertyCommand.VideoUrls = new List<string>();
            }
            await _mediator.Send(PropertyCommand);
            return RedirectToPage("./Index");
        }
    }
}
