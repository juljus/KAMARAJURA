namespace backend.Models
{
    public class User
    {
        public int user_id { get; set; } // Primary key
        public string user_name { get; set; } = string.Empty; // Username
        public string user_password { get; set; } = string.Empty; // Password
    }
}