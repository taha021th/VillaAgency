using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VillaAgency.Application.Handlers.ContactMessages.Commands;
using VillaAgency.Application.Handlers.Properties.Queries;
using VillaAgency.Application.Handlers.ViewModels;

namespace VillaAgency.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator=mediator;
        }
        public HomePageViewModel Vm { get; set; }

        #region Submit message

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "وارد کردن نام الزامی است.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
            [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد کردن موضوع الزامی است.")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "وارد کردن پیام الزامی است.")]
            public string Message { get; set; }
        }

        #endregion

        public async Task OnGetAsync()
        {
            Vm=await _mediator.Send(new GetHomePageDataQuery());
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // قبل از ارسال فرم، باید اطلاعات اولیه صفحه را دوباره بارگذاری کنیم
            // تا در صورت وجود خطا، صفحه به درستی نمایش داده شود.
            Vm = await _mediator.Send(new GetHomePageDataQuery());

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var command = new CreateContactMessageCommand
            {
                Name = Input.Name,
                Email = Input.Email,
                Subject = Input.Subject,
                Message = Input.Message,
            };

            await _mediator.Send(command);
            StatusMessage = "پیام شما با موفقیت ارسال شد. با تشکر";

            // برای جلوگیری از ارسال مجدد فرم با رفرش صفحه، از Redirect استفاده می‌کنیم
            return RedirectToPage("/Index");
        }
    }
}
