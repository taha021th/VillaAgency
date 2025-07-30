using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VillaAgency.Application.Handlers.ContactMessages.Commands;

namespace VillaAgency.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IMediator _mediator;
        public ContactModel(IMediator mediator)
        {
            _mediator= mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "واد کردن نام الزامی است.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
            [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد کردن موضوع الزامی است.")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "وارد کردن پیام الزامی است.")]
            public string Message { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var command = new CreateContactMessageCommand
            {
                Name=Input.Name,
                Email=Input.Email,
                Subject=Input.Subject,
                Message=Input.Message,

            };
            await _mediator.Send(command);
            StatusMessage="پیام شما با موفقیت ارسال شد. با تشکر";
            return RedirectToPage();
        }
    }
}
