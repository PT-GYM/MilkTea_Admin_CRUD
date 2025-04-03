using BussinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace MilkTea_Customer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = await _accountService.Login(Username, Password);          
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }

            if (user.Role != "Customer")
            {
                ViewBag.ErrorMessage = "You have no permission to access this page.";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return RedirectToAction("Show", "Menu");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Username, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            var newUser = new User
            {
                Username = Username,
                Password = Password,
                Role = "Customer"
            };

            var success = await _accountService.Register(newUser);
            if (!success)
            {
                ViewBag.ErrorMessage = "Username already exists.";
                return View();
            }

            return RedirectToAction("Login");
        }
    }
}
