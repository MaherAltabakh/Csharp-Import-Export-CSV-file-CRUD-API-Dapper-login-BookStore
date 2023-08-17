using BookStore.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace BookStore.Controllers
{
    public class book : Controller
    {
        public IActionResult ExportToCSV()
        {
            HttpClient client = new HttpClient();
            var responseTask = client.GetAsync("https://localhost:7068/api/BookAPI");
            responseTask.Wait();
            if (responseTask.IsCompleted)
            {
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var MessageTask = result.Content.ReadAsStringAsync();
                    var str = MessageTask.Result;
                    var books = JsonConvert.DeserializeObject<List<Models.Book>>(MessageTask.Result);

                    if (books.Count > 0)
                    {
                        using (var writer = new StreamWriter("books.csv"))
                        {
                            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                            {
                                csv.Context.RegisterClassMap<BookMap>();
                                csv.WriteRecords(books);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("ReadBooks");
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromCSV()
        {
            try
            {
                if (Request.Form.Files[0] != null)
                {
                    var importedData = Request.Form.Files[0];
                    //var fullPath = Path.GetFullPath(importedData.FileName);
                    using (var streamReader = new StreamReader(importedData.OpenReadStream()))
                    {
                        using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                        {
                            //csv.Context.RegisterClassMap<BookMap>(); // Register mapping
                            csv.Context.RegisterClassMap<BookMapReading>(); // Register mapping
                            try
                            {
                                var record = csv.GetRecords<Book>().ToList();
                                for (int i = 0; i < record.Count; i++)
                                {
                                    record[i].BookID = 0;
                                    SaveCreateAndUpdateBook(record[i]);
                                }
                            }
                            catch (CsvHelper.MissingFieldException missingFieldEx)
                            {
                                // Log or display missing field exception details
                                Console.WriteLine("Missing Field Exception: " + missingFieldEx.Message);
                                Console.WriteLine("Stack Trace: " + missingFieldEx.StackTrace);
                            }

                        }
                    }
                }
                else
                {
                    TempData["FileErrorMessage"] = "No File was uploaded";
                }
            }
            catch (Exception ex)
            {
                TempData["FileErrorMessage"] = "Error occurred while processing the file";
                Console.WriteLine("General Exception: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
            return RedirectToAction("ReadBooks");
        }

        public IActionResult CreateAndUpdateBook(Models.Book book)
        {
            ViewBag.errorMessage = TempData["errorMessage"];

            if ((ViewBag.errorMessage == null) && (book.BookID != 0))
            {
                HttpClient client = new HttpClient();
                var responseTask = client.GetAsync("https://localhost:7068/api/BookAPI/" + book.BookID);
                responseTask.Wait();
                if (responseTask.IsCompleted)
                {
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var MessageTask = result.Content.ReadAsStringAsync();
                        var str = MessageTask.Result;
                        var bookToUpdate = JsonConvert.DeserializeObject<Models.Book>(MessageTask.Result);

                        ViewBag.Book = bookToUpdate;
                    }
                }
            }
            else
                ViewBag.Book = book;

            return View();
        }

        public IActionResult SaveCreateAndUpdateBook(Models.Book book)
        {
            List<string> errorMessages = new List<string>();
            BookDB bookDb = new BookDB();

            if (book.Title == null)
                errorMessages.Add("Title is required");
            if (book.NumberofPages == 0)
                errorMessages.Add("Number of pages is required");

            if (errorMessages.Count > 0)
            {
                TempData["errorMessage"] = errorMessages;
                //here we are redirecting to the above action and pass the mistaken object to send it back to the HTML page to be edited
                return RedirectToAction("CreateAndUpdateBook", book);
            }

            if (book.BookID == 0)
            {
                book.CreatedDate = DateTime.Now;
                //call the API create action
                HttpClient client = new HttpClient();
                var responseTask = client.PostAsJsonAsync("https://localhost:7068/api/BookAPI/", book);
                responseTask.Wait();
            }
            else
            {
                book.UpdatedDate = DateTime.Now;
                //call the API update action
                HttpClient client = new HttpClient();
                var responseTask = client.PutAsJsonAsync("https://localhost:7068/api/BookAPI/" + book.BookID, book);
                responseTask.Wait();
            }

            return RedirectToAction("ReadBooks");
        }

        public IActionResult ReadBooks()
        {
            ViewBag.FileErrorMessage = TempData["FileErrorMessage"];

            HttpClient client = new HttpClient();
            var responseTask = client.GetAsync("https://localhost:7068/api/BookAPI");
            responseTask.Wait();
            if (responseTask.IsCompleted)
            {
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var MessageTask = result.Content.ReadAsStringAsync();
                    var str = MessageTask.Result;
                    var books = JsonConvert.DeserializeObject<List<Models.Book>>(MessageTask.Result);
                    ViewBag.Book = books;
                }
            }
            return View();
        }

        public IActionResult DeleteBook(int BookID)
        {
            HttpClient client = new HttpClient();
            var responseTask = client.DeleteAsync("https://localhost:7068/api/BookAPI/" + BookID);
            responseTask.Wait();

            return RedirectToAction("ReadBooks");
        }
    }
}