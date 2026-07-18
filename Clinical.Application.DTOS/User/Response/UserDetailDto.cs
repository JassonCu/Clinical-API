namespace Clinical.Application.DTOS.User.Response
{
    public class UserDetailDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RoleName { get; set; }
        public int RoleId { get; set; }
        public int State { get; set; }
        public DateTime AuditCreateDate { get; set; }
    }
}
