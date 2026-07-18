using Clinical.UseCases.Commons.Bases;
using MediatR;
using System.Text.Json.Serialization;

namespace Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommand : IRequest<BaseResponse<bool>>
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }

        /// <summary>
        /// Resolved server-side from the authenticated token. Marked <see cref="JsonIgnoreAttribute"/>
        /// so a client can never influence which account is targeted (prevents the previous IDOR).
        /// </summary>
        [JsonIgnore]
        public string Username { get; set; } = string.Empty;
    }
}
