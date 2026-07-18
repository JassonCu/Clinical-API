namespace Clinical.Domain.Entities
{
    public class User
    {
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? RoleId { get; set; }
        public int? DoctorId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? State { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public bool? MustChangePassword { get; set; }
    }
}
