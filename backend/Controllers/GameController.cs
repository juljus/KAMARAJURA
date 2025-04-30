using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly string _connectionString;

        public GameController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' is missing or null.");

            // Removed custom type mapping as models now match the database schema
        }

        [HttpGet]
        public IActionResult GetAllGames()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var games = connection.Query<Game>("SELECT * FROM dbo.games");
                    Console.WriteLine("Games retrieved from database:", games);
                    return Ok(games);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving games:", ex.Message);
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var game = connection.QueryFirstOrDefault<Game>("SELECT * FROM dbo.games WHERE game_id = @Id", new { Id = id });
                    if (game == null)
                    {
                        return NotFound();
                    }
                    return Ok(game);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateGame([FromBody] Game game)
        {
            if (game == null || string.IsNullOrWhiteSpace(game.game_name))
            {
                return BadRequest("Invalid game data. GameName is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Added validation to check for duplicate games before inserting
                    var existingGame = connection.QueryFirstOrDefault<Game>("SELECT game_id FROM dbo.games WHERE game_name = @game_name", new { game_name = game.game_name });
                    if (existingGame != null)
                    {
                        return Conflict("A game with the same name already exists in the database.");
                    }

                    // Updated SQL query to use SCOPE_IDENTITY() for generating game_id
                    var sql = "INSERT INTO dbo.games (game_name, game_description) VALUES (@game_name, @game_description); SELECT CAST(SCOPE_IDENTITY() as int)";
                    int newGameId = connection.ExecuteScalar<int>(sql, new { game.game_name, game.game_description });
                    game.game_id = newGameId;
                    return CreatedAtAction(nameof(GetGameById), new { id = newGameId }, game);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGame(int id, [FromBody] Game game)
        {
            if (game == null || string.IsNullOrWhiteSpace(game.game_name))
            {
                return BadRequest("Invalid game data. GameName is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Removed GameImage reference from the UpdateGame method
                    var sql = "UPDATE dbo.games SET game_name = @game_name, game_description = @game_description WHERE game_id = @game_id";
                    int rowsAffected = connection.Execute(sql, new { game.game_name, game.game_description, game_id = id });

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGame(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = "DELETE FROM dbo.games WHERE game_id = @Id";
                    int rowsAffected = connection.Execute(sql, new { Id = id });

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }
    }
}