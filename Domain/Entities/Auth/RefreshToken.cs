namespace Domain.Entities.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public DateTime Expires { get; set; }

        public bool Enabled { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
