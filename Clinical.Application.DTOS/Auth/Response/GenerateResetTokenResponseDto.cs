namespace Clinical.Application.DTOS.Auth.Response
{
    public class GenerateResetTokenResponseDto
    {
        public string RawToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string TargetUsername { get; set; } = string.Empty;
    }
}
