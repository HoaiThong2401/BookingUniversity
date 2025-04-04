using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System;

namespace BookingManagementRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("Email", "Email is required");
                ModelState.AddModelError("Password", "Password is required");
                return Page();
            }

            var user = _userService.GetAccountMemberByEmail(Email, Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or password not available!");
                ErrorMessage = "Email or Password is incorrect. Please check again!";
                return Page();
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (user.RoleId == 3)
                {
                    string redirectUrl = $"http://localhost:5196/TeacherHomePage?userId={user.Id}&roleId={user.RoleId}";
                    return Redirect(redirectUrl);
                }
                else if (user.RoleId == 1)
                {
                    return RedirectToPage("/Manager/ManagerHomePage");
                }
                else
                {
                    return RedirectToPage("/HeadDepartment/HeadDepartmentHomePage");
                }
            }
        }
    }
}
