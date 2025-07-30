using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList RoleList { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "نام کامل الزامی است.")]
            [Display(Name = "نام کامل")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "ایمیل الزامی است.")]
            [EmailAddress]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Display(Name = "شماره تلفن")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "رمز عبور الزامی است.")]
            [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} کاراکتر باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "رمز عبور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار رمز عبور")]
            [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن با هم مطابقت ندارند.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "انتخاب نقش الزامی است.")]
            [Display(Name = "نقش")]
            public string SelectedRole { get; set; }

            [Display(Name = "عکس پروفایل")]
            public IFormFile ProfileImage { get; set; }
        }

        public void OnGet()
        {
            // لیست نقش‌ها را برای نمایش در فرم آماده می‌کنیم
            RoleList = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // اگر فرم نامعتبر بود، لیست نقش‌ها را دوباره بارگذاری می‌کنیم
                RoleList = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber,
                EmailConfirmed = true // برای سادگی، ایمیل را تایید شده در نظر می‌گیریم
            };

            // آپلود عکس پروفایل در صورت وجود
            if (Input.ProfileImage != null)
            {
                string folder = "images/agents/";
                string fileName = Guid.NewGuid().ToString() + "_" + Input.ProfileImage.FileName;
                string serverFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                // اگر پوشه وجود نداشت، آن را ایجاد کن
                if (!Directory.Exists(serverFolderPath))
                {
                    Directory.CreateDirectory(serverFolderPath);
                }

                string imagePath = Path.Combine(serverFolderPath, fileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await Input.ProfileImage.CopyToAsync(fileStream);
                }
                user.ProfileImageUrl = fileName; // فقط نام فایل را ذخیره می‌کنیم
            }

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // تخصیص نقش انتخاب شده به کاربر
                await _userManager.AddToRoleAsync(user, Input.SelectedRole);
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // در صورت خطا، لیست نقش‌ها را دوباره بارگذاری کن
            RoleList = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return Page();
        }
    }
}