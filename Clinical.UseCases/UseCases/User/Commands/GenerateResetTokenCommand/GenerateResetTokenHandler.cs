using System.Security.Cryptography;
using System.Text;
using Clinical.Application.DTOS.Auth.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.GenerateResetTokenCommand
{
    public class GenerateResetTokenHandler : IRequestHandler<GenerateResetTokenCommand, BaseResponse<GenerateResetTokenResponseDto>>
    {
        private readonly IPasswordResetRepository _resetRepo;
        private readonly IUserRepository _userRepo;

        public GenerateResetTokenHandler(IPasswordResetRepository resetRepo, IUserRepository userRepo)
        {
            _resetRepo = resetRepo;
            _userRepo = userRepo;
        }

        public async Task<BaseResponse<GenerateResetTokenResponseDto>> Handle(GenerateResetTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GenerateResetTokenResponseDto>();

            var user = await _userRepo.GetUserByIdAsync(request.UserId);
            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado.";
                return response;
            }

            var rawBytes = new byte[32];
            RandomNumberGenerator.Fill(rawBytes);
            var rawToken = Convert.ToBase64String(rawBytes);

            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            var tokenHash = Convert.ToHexString(hashBytes).ToLower();

            var expiresAt = DateTime.UtcNow.AddHours(24);
            await _resetRepo.CreateTokenAsync(request.UserId, tokenHash, expiresAt);

            response.IsSuccess = true;
            response.Message = "Token generado. Compártalo de forma segura con el usuario.";
            response.Data = new GenerateResetTokenResponseDto
            {
                RawToken = rawToken,
                ExpiresAt = expiresAt,
                TargetUsername = user.Username ?? string.Empty
            };

            return response;
        }
    }
}
