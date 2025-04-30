using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly string _connectionString;

        public RatingController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' is missing or null.");

            // Removed custom type mapping as models now match the database schema
        }

        [HttpGet]
        public IActionResult GetAllRatings()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var ratings = connection.Query<Rating>("SELECT * FROM dbo.ratings");
                    return Ok(ratings);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRatingById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var rating = connection.QueryFirstOrDefault<Rating>("SELECT * FROM dbo.ratings WHERE rating_id = @Id", new { Id = id });
                    if (rating == null)
                    {
                        return NotFound();
                    }
                    return Ok(rating);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("game/{gameId}")]
        public IActionResult GetReviewsByGameId(int gameId)
        {
            Console.WriteLine($"Fetching reviews for gameId: {gameId}");
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var reviews = connection.Query("SELECT r.*, u.user_name AS username FROM dbo.ratings r JOIN dbo.users u ON r.user_id = u.user_id WHERE r.game_id = @GameId", new { GameId = gameId });
                    Console.WriteLine("Reviews fetched from database:");
                    foreach (var review in reviews)
                    {
                        Console.WriteLine($"Rating ID: {review.rating_id}, Score: {review.rating_score}, Description: {review.rating_description}");
                    }
                    return Ok(reviews);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reviews: {ex.Message}");
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateRating([FromBody] Rating rating)
        {
            if (rating == null || rating.game_id <= 0 || rating.user_id <= 0 || rating.rating_score <= 0)
            {
                return BadRequest("Invalid rating data. GameId, UserId, and RatingScore are required.");
            }

            if (rating.rating_timestamp == default)
            {
                rating.rating_timestamp = DateTime.UtcNow;
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Added validation to ensure game_id and user_id exist before creating a rating
                    var gameExists = connection.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM dbo.games WHERE game_id = @game_id", new { game_id = rating.game_id });
                    if (gameExists == 0)
                    {
                        return BadRequest("Invalid game_id. The game does not exist.");
                    }

                    var userExists = connection.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM dbo.users WHERE user_id = @user_id", new { user_id = rating.user_id });
                    if (userExists == 0)
                    {
                        return BadRequest("Invalid user_id. The user does not exist.");
                    }

                    // Added validation to ensure a user can only rate a game once
                    var existingRating = connection.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM dbo.ratings WHERE game_id = @game_id AND user_id = @user_id", new { game_id = rating.game_id, user_id = rating.user_id });
                    if (existingRating > 0)
                    {
                        return Conflict("This user has already rated this game.");
                    }

                    var sql = "INSERT INTO dbo.ratings (game_id, user_id, rating_timestamp, rating_score, rating_description) VALUES (@game_id, @user_id, @rating_timestamp, @rating_score, @rating_description); SELECT CAST(SCOPE_IDENTITY() as int)";
                    int newRatingId = connection.ExecuteScalar<int>(sql, rating);
                    rating.rating_id = newRatingId;
                    return CreatedAtAction(nameof(GetRatingById), new { id = newRatingId }, rating);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRating(int id, [FromBody] Rating rating)
        {
            if (rating == null || rating.rating_score <= 0)
            {
                return BadRequest("Invalid rating data. RatingScore is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = "UPDATE dbo.ratings SET game_id = @GameId, user_id = @UserId, rating_timestamp = @RatingTimestamp, rating_score = @RatingScore, rating_description = @RatingDescription WHERE rating_id = @RatingId";
                    int rowsAffected = connection.Execute(sql, new { rating.game_id, rating.user_id, rating.rating_timestamp, rating.rating_score, rating.rating_description, rating_id = id });

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
        public IActionResult DeleteRating(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = "DELETE FROM dbo.ratings WHERE rating_id = @Id";
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