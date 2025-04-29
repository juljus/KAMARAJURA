namespace backend.Models
{
    public class Game
    {
        public int GameId { get; set; } // Primary key
        public string GameName { get; set; } = string.Empty; // Name of the game
        public string GameDescription { get; set; } = string.Empty; // Description of the game
    }
}