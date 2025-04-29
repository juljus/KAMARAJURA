namespace backend.Models
{
    public class Rating
    {
        public int RatingId { get; set; } // Primary key
        public int GameId { get; set; } // Foreign key referencing Game
        public int UserId { get; set; } // Foreign key referencing User
        public DateTime RatingTimestamp { get; set; } // Timestamp of the rating
        public int RatingScore { get; set; } // Score given by the user
        public string RatingDescription { get; set; } = string.Empty; // Description or comment for the rating
    }
}