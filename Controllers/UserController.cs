using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Controllers
{


    public class UserController : Controller
    {
        public IActionResult CreateAndUpdateUser(User user)
        {
            List<string> errorMessages = new List<string>();

            if (user.FirstName == null) errorMessages.Add("First Name is required");
            if (user.LastName == null) errorMessages.Add("Last Name is required");

            if (user.Email == null) errorMessages.Add("Email is required");
            else//check if the email already existed in the Database
            {
                using (HttpClient client = new HttpClient())
                {
                    var responseTask = client.GetAsync($"https://localhost:7068/api/UserAPI/GetByEmail/{user.Email}");
                    responseTask.Wait();
                    if (responseTask.IsCompleted)
                        if (responseTask.Result.IsSuccessStatusCode)
                            errorMessages.Add("Email is already existed!");
                }
            }

            if (user.Phone == null) errorMessages.Add("Phone is required");
            else//check if the Phone already existed in the Database
            {
                using (HttpClient client = new HttpClient())
                {
                    var responseTask = client.GetAsync($"https://localhost:7068/api/UserAPI/GetByPhone/{user.Phone}");
                    responseTask.Wait();
                    if (responseTask.IsCompleted)
                        if (responseTask.Result.IsSuccessStatusCode)
                            errorMessages.Add("Phone is already existed!");
                }
            }

            if (user.Password == null) errorMessages.Add("Password is required");
            if (errorMessages.Count > 0)
            {
                TempData["errorMessage"] = errorMessages;
                //here we are redirecting to the above action and pass the mistaken object to send it back to the HTML page to be edited
                return RedirectToAction("NewUserRegister", "Login", user);
            }

            if (user.UserID == 0)
            {
                //call the API create action
                HttpClient client = new HttpClient();
                var responseTask = client.PostAsJsonAsync("https://localhost:7068/api/UserAPI/", user);
                responseTask.Wait();
            }

            else
            {
                //call the API update action
                HttpClient client = new HttpClient();
                var responseTask = client.PutAsJsonAsync("https://localhost:7068/api/UserAPI/" + user.UserID, user);
                responseTask.Wait();
            }
            return RedirectToAction("LoginAction", "Login");
        }

        public IActionResult ReadUser(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                var responseTask = client.GetAsync($"https://localhost:7068/api/BookAPI/{Id}");
                responseTask.Wait();
                if (responseTask.IsCompleted)
                {
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var MessageTask = result.Content.ReadAsStringAsync();
                        var str = MessageTask.Result;
                        var user = JsonConvert.DeserializeObject<User>(MessageTask.Result);
                        ViewBag.User = user;
                    }
                }
            }
            return View();
        }






        //public IActionResult DeleteBook(int BookID)
        //{
        //    HttpClient client = new HttpClient();
        //    var responseTask = client.DeleteAsync("https://localhost:7068/api/BookAPI/" + BookID);
        //    responseTask.Wait();

        //    return RedirectToAction("ReadBooks");
        //}
    }

}
