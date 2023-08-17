namespace BookStore.Models
{
    public class LoginDB
    {
        public List<string> signin(string Email, string Password)
        {

            List<string> errorMessages = new List<string>();
            //check if the Email and password matched
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://localhost:7068/api/UserAPI/GetByEmailAndPassword/{Email}/{Password}";
                var responseTask = client.GetAsync(apiUrl);
                responseTask.Wait();
                if (responseTask.IsCompleted)
                    if (!responseTask.Result.IsSuccessStatusCode)
                    { //check if the email already existed in the Database
                        using (HttpClient client1 = new HttpClient())
                        {
                            var checkEmailresponse = client1.GetAsync($"https://localhost:7068/api/UserAPI/GetByEmail/{Email}");
                            checkEmailresponse.Wait();
                            if (checkEmailresponse.IsCompleted)
                                if (checkEmailresponse.Result.IsSuccessStatusCode)
                                    errorMessages.Add("Username and password not matched");
                                else
                                    errorMessages.Add("Username is Not existed! <br /> please create and account");
                        }
                    }
            }
            return errorMessages;
        }


    }
}
