using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Security.Claims;

namespace MilkTea_Admin_CRUD.Pages.Authen
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly IAccount _accountService;

        public LoginModel(IAccount accountService)
        {
            _accountService = accountService;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessages { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToPage("./Authen");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessages = "Email and Password must be required!";
                return Page();
            }

            var acc = await _accountService.Login(Username, Password);
            if (acc == null)
            {
                Console.WriteLine("Login thất bại: Không tìm thấy tài khoản");
                ErrorMessages = "This account don't exist!";
                return Page();
            }
            Console.WriteLine($"Login thành công: {acc.Username} - Role: {acc.Role}");
            if (acc.Role != "Admin")
            {
                ErrorMessages = "You do not have permission to do this function!";
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", acc.Username);
            HttpContext.Session.SetString("Role", acc.Role);
            HttpContext.Session.SetString("UserPassword", acc.Password);


            return RedirectToPage("/ProductPage/Show");
        }
    }
}



