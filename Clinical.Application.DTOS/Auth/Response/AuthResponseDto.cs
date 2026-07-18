namespace Clinical.Application.DTOS.Auth.Response
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
