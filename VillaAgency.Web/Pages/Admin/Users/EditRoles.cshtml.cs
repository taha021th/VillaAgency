using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Web.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class EditRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EditRolesModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }

        [BindProperty]
        public List<RoleViewModel> Roles { get; set; }

        public class RoleViewModel
        {
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserId = user.Id.ToString();
            UserName = user.UserName;
            Roles = new List<RoleViewModel>();

            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in allRoles)
            {
                Roles.Add(new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var roleViewModel in Roles)
            {
                // اگر نقش انتخاب شده و کاربر آن را ندارد، اضافه کن
                if (roleViewModel.IsSelected && !userRoles.Contains(roleViewModel.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, roleViewModel.RoleName);
                }
                // اگر نقش انتخاب نشده و کاربر آن را دارد، حذف کن
                else if (!roleViewModel.IsSelected && userRoles.Contains(roleViewModel.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, roleViewModel.RoleName);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
