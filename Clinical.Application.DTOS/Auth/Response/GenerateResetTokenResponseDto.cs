namespace Clinical.Application.DTOS.Auth.Response
{
    public class GenerateResetTokenResponseDto
    {
        /// <summary>The raw token, returned only when it could not be delivered to the user directly.</summary>
        public string? RawToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TargetUsername { get; set; } = string.Empty;
    }
}
