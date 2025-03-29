using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Invalid user data. Name and Password are required.");
            }

            // Check for duplicate users (e.g., by Name or other unique property)
            if (users.Any(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict("A user with the same name already exists.");
            }

            // Assign a new ID
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

            // TODO: Hash the password before storing it (or hash it before sending it to the backend)

            // Add the user to the list
            users.Add(user);

            // Return the created user with a 201 Created response
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
    }
}