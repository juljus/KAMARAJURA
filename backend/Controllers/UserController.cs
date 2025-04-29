using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using BCrypt.Net;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' is missing or null.");
        }

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

        // [HttpPost]
        // public ActionResult<User> Create([FromBody] User user)
        // {
        //     // Ensure the user object is not null and has required properties
        //     if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
        //     {
        //         return BadRequest("Invalid user data. Name and Password are required.");
        //     }

        //     // Check for duplicate users (e.g., by Name or other unique property)
        //     if (users.Any(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)))
        //     {
        //         return Conflict("A user with the same name already exists.");
        //     }

        //     // Assign a new ID
        //     user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

        //     // TODO: Hash the password before storing it (or hash it before sending it to the backend)

        //     // Add the user to the list
        //     users.Add(user);

        //     // Return the created user with a 201 Created response
        //     return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        // }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Invalid user data. Name and Password are required.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Check for duplicate users in the database
                    var existingUser = connection.QueryFirstOrDefault<User>("SELECT id FROM dbo.users WHERE user_name = @Name", new { Name = user.Name });
                    if (existingUser != null)
                    {
                        return Conflict("A user with the same name already exists in the database.");
                    }

                    // TODO: Hash the password BEFORE storing it
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    // Insert the new user into the database
                    var sql = "INSERT INTO dbo.users (user_name, user_password) VALUES (@Name, @PasswordHash); SELECT CAST(SCOPE_IDENTITY() as int)";
                    int newUserId = connection.ExecuteScalar<int>(sql, new { Name = user.Name, PasswordHash = hashedPassword });

                    // Retrieve the newly created user (optional, but good practice)
                    var createdUser = connection.QueryFirstOrDefault<User>("SELECT id, user_name FROM dbo.users WHERE id = @Id", new { Id = newUserId });

                    if (createdUser != null)
                    {
                        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
                    }
                    else
                    {
                        return StatusCode(500, "Failed to retrieve the newly created user.");
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}