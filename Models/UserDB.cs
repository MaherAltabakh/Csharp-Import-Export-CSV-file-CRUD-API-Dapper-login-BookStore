using Dapper;
using System.Data.SqlClient;

namespace BookStore.Models
{
    public class UserDB
    {
        private string connectionString = ("server = Administrator; database = BookStore; user id = reader; password = pass123;");
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string sql = "select * from [User]";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                users = conn.Query<User>(sql).ToList();
            }
            return users;
        }

        public User GetUserById(int UserID)
        {
            var user = new User();
            string sql = " select * from [User] WHERE UserID =@UserID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                user = conn.QueryFirst<User>(sql, new { UserID });
            }

            return user;
        }

        public User GetUserByEmail(string Email)
        {
            var user = new User();
            string sql = " select * from [User] WHERE Email =@Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                user = conn.QueryFirst<User>(sql, new { Email });
            }
            return user;
        }




        public User GetUserByEmailAndPassword(string Email, string Password)
        {
            var user = new User();
            string sql = " select * from [User] WHERE (Email =@Email and Password =@Password) ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                user = conn.QueryFirst<User>(sql, new { Email, Password });
            }
            return user;
        }



        public User GetUserByPhone(string Phone)
        {
            var user = new User();
            string sql = " select * from [User] WHERE Phone =@Phone";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                user = conn.QueryFirst<User>(sql, new { Phone });
            }
            return user;
        }
        public void DeleteUser(int UserID)
        {
            string sql = "DELETE FROM [User] WHERE [UserID] = @UserID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(sql, new { UserID });
            }

        }

        public void UserEditSave(User user)
        {
            string sql;
            if (user.UserID == 0)
            {
                sql = "INSERT INTO [User] " +
                        "(FirstName, LastName, Email, Phone, Password)" +
                        "VALUES" +
                        "(@FirstName, @LastName, @Email, @Phone, @Password)";
            }
            else
            {
                sql = "UPDATE [User] SET " +
                         "(FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, Password=@Password)" +
                         "WHERE UserID = @UserID;";

            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(sql, user);
            }
        }

    }
}
