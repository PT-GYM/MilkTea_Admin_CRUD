﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services.Interface;

namespace MilkTea_Customer_.Controllers
{
    public class AuthenController : Controller
    {
        private readonly IAccountService _accountService;

        public AuthenController(IAccountService accountService)
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

            if (user.Role !="Customer")
            {
                ViewBag.ErrorMessage = "You have no permission to access this page.";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Login", "Authen");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Authen");
        }
    }
}
