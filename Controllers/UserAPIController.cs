using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        // GET: api/<BookAPIController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            UserDB userDb = new UserDB();
            List<User> users = userDb.GetAllUsers();
            return users;
        }

        // GET api/<BookAPIController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            UserDB userDb = new UserDB();
            var user = userDb.GetUserById(id);
            return user;
        }



        // GET api/<BookAPIController>/5
        [HttpGet("GetByEmail/{Email}")]
        public async Task<ActionResult<User>> GetByEmail(string Email)
        {
            UserDB userDb = new UserDB();
            var user = userDb.GetUserByEmail(Email);
            return user;
        }

        // GET api/<BookAPIController>/5
        [HttpGet("GetByPhone/{Phone}")]
        public async Task<ActionResult<User>> GetByPhone(string Phone)
        {
            UserDB userDb = new UserDB();
            var user = userDb.GetUserByPhone(Phone);
            return user;
        }

        // GET api/<BookAPIController>/5
        [HttpGet("GetByEmailAndPassword/{Email}/{Password}")]
        public async Task<ActionResult<User>> GetByEmailAndPassword(string Email, string Password)
        {
            UserDB userDb = new UserDB();
            var user = userDb.GetUserByEmailAndPassword(Email, Password);
            return user;
        }

        // POST api/<BookAPIController>
        [HttpPost]
        public void AddUsers([FromBody] User user)
        {
            UserDB userDb = new UserDB();
            userDb.UserEditSave(user);
        }

        // PUT api/<BookAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            UserDB userDb = new UserDB();
            userDb.UserEditSave(user);
        }

        // DELETE api/<BookAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            UserDB userDb = new UserDB();
            userDb.DeleteUser(id);
        }
    }
}