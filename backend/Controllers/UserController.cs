using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.Data.SqlClient;
using Dapper;

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
                    var existingUser = connection.QueryFirstOrDefault<User>("SELECT user_id FROM dbo.users WHERE user_name = @Name", new { Name = user.Name });
                    if (existingUser != null)
                    {
                        return Conflict("A user with the same name already exists in the database.");
                    }

                    // TODO: Hash the password BEFORE storing it
                    string hashedPassword = HashPassword(user.Password); // Replace with your hashing logic

                    // Insert the new user into the database
                    var sql = "INSERT INTO dbo.users (user_name, user_password) VALUES (@Name, @PasswordHash); SELECT CAST(SCOPE_IDENTITY() as int)";
                    int newUserId = connection.ExecuteScalar<int>(sql, new { Name = user.Name, PasswordHash = hashedPassword });

                    // Retrieve the newly created user (optional, but good practice)
                    var createdUser = connection.QueryFirstOrDefault<User>("SELECT user_id, user_name FROM dbo.users WHERE user_id = @Id", new { Id = newUserId });

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

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return Ok("Connection successful!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Connection failed: {ex.Message}");
            }
        }

        // Placeholder for password hashing (you'll need a proper implementation)
        private string HashPassword(string password)
        {
            // In a real application, use a robust hashing algorithm like BCrypt or Argon2
            return password; // THIS IS NOT SECURE!
        }
    }
}