using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VillaAgency.Application.Handlers.Categories.Queries;
using VillaAgency.Application.Handlers.PropertyRequests.Commands;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Web.Pages
{
    public class PropertyRequestModel : PageModel
    {
        private readonly IMediator _mediator;
        public PropertyRequestModel(IMediator mediator)
        {
            _mediator= mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<Category> CategoryList { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "لطفا نام و نام خانوادگی را وارد کنید.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "لطفا شماره تماس را وارد کنید.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "لطفا نوع معامله را مشخص کنید.")]
            public string TransactionType { get; set; }

            [Required(ErrorMessage = "حداقل یک نوع ملک را انتخاب کنید.")]
            public List<Guid> CategoryIds { get; set; } = new();

            [Required(ErrorMessage = "لطفا مناطق مورد نظر را وارد کنید.")]
            public string DesiredRegions { get; set; }

            public decimal? MinBudget { get; set; }
            public decimal? MaxBudget { get; set; }
            public int? MinArea { get; set; }
            public int? MaxArea { get; set; }
            public int? MinBedrooms { get; set; }
            public string? Description { get; set; }
        }

        public async Task OnGetAsync()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            CategoryList = categories.ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            var command = new CreatePropertyRequestCommand
            {
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber,
                TransactionType = Input.TransactionType,
                CategoryIds = Input.CategoryIds,
                DesiredRegions = Input.DesiredRegions,
                MinBudget = Input.MinBudget,
                MaxBudget = Input.MaxBudget,
                MinArea = Input.MinArea,
                MaxArea = Input.MaxArea,
                MinBedrooms = Input.MinBedrooms,
                Description = Input.Description

            };
            await _mediator.Send(command);
            TempData["SuccessMessage"]="درخواست شما با موفقیت ثبت شد. کارشناسان ما پس از بررسی با شمال تماس خواهند گرفت.";
            return RedirectToPage();
        }
    }
}
