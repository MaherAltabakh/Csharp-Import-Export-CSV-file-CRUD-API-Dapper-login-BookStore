using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class Login : Controller
    {
        public IActionResult LoginAction(User user)
        {
            if (user.Email != null || user.Password != null)
            {
                List<string> errorMessages = new List<string>();
                LoginDB logindb = new LoginDB();
                errorMessages = logindb.signin(user.Email, user.Password);
                if (errorMessages.Count == 0) return RedirectToAction("ReadBooks", "book");
                ViewBag.ErrorMessage = errorMessages.ToArray();
            }
            return View();
        }

        public IActionResult NewUserRegister(User user)
        {
            ViewBag.user = user;
            ViewBag.errors = TempData["errorMessage"];

            return View();
        }

        public IActionResult Logout()
        {
            return View("LoginAction");
        }

    }
}
