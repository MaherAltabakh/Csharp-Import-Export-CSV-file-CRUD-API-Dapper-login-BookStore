using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class User
    {
            public int UserID { set; get; }
            public string FirstName { set; get; }
            public string LastName { set; get; }
            public string Email { set; get; }
            public string Phone { set; get; }
            public string Password { set; get; }



    }
}
