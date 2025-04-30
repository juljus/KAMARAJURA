namespace backend.Models
{
    public class Game
    {
        public int game_id { get; set; } // Primary key
        public string game_name { get; set; } = string.Empty; // Name of the game
        public string game_description { get; set; } = string.Empty; // Description of the game
    }
}