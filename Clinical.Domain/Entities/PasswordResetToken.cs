namespace Clinical.Domain.Entities
{
    public class PasswordResetToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
