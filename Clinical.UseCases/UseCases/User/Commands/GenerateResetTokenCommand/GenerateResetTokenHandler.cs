using System.Security.Cryptography;
using Clinical.Application.DTOS.Auth.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clinical.UseCases.UseCases.User.Commands.GenerateResetTokenCommand
{
    public class GenerateResetTokenHandler : IRequestHandler<GenerateResetTokenCommand, BaseResponse<GenerateResetTokenResponseDto>>
    {
        private readonly IPasswordResetRepository _resetRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordResetNotifier _notifier;
        private readonly ILogger<GenerateResetTokenHandler> _logger;

        public GenerateResetTokenHandler(
            IPasswordResetRepository resetRepo,
            IUserRepository userRepo,
            IPasswordResetNotifier notifier,
            ILogger<GenerateResetTokenHandler> logger)
        {
            _resetRepo = resetRepo;
            _userRepo = userRepo;
            _notifier = notifier;
            _logger = logger;
        }

        public async Task<BaseResponse<GenerateResetTokenResponseDto>> Handle(GenerateResetTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GenerateResetTokenResponseDto>();

            var user = await _userRepo.GetUserByIdAsync(request.UserId)
                ?? throw new NotFoundException("Usuario", request.UserId);

            var rawBytes = new byte[32];
            RandomNumberGenerator.Fill(rawBytes);
            var rawToken = Convert.ToBase64String(rawBytes);

            var expiresAt = DateTime.UtcNow.AddHours(24);
            // Only the hash is persisted (never the raw token).
            await _resetRepo.CreateTokenAsync(request.UserId, TokenHasher.Hash(rawToken), expiresAt);

            // Audit this sensitive action — never log the raw token.
            _logger.LogWarning("Password reset token generated for user {UserId} ({Username}); expires {ExpiresAt:o}.",
                request.UserId, user.Username, expiresAt);

            // Prefer direct delivery to the user; only fall back to returning the token to the caller.
            var delivered = await _notifier.TryNotifyAsync(user.Username ?? string.Empty, user.Email, rawToken, expiresAt, cancellationToken);

            response.IsSuccess = true;
            response.Message = delivered
                ? "Se envió el enlace de restablecimiento al usuario."
                : "Token generado. Compártalo de forma segura con el usuario.";
            response.Data = new GenerateResetTokenResponseDto
            {
                RawToken = delivered ? null : rawToken,
                ExpiresAt = expiresAt,
                TargetUsername = user.Username ?? string.Empty
            };

            return response;
        }
    }
}
