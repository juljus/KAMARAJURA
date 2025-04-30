namespace backend.Models
{
    public class Rating
    {
        public int rating_id { get; set; } // Primary key
        public int game_id { get; set; } // Foreign key referencing Game
        public int user_id { get; set; } // Foreign key referencing User
        public DateTime rating_timestamp { get; set; } // Timestamp of the rating
        public int rating_score { get; set; } // Score given by the user
        public string rating_description { get; set; } = string.Empty; // Description or comment for the rating
    }
}