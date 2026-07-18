using System.Security.Cryptography;
using System.Text;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.ResetPasswordCommand;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, BaseResponse<bool>>
{
    private readonly IPasswordResetRepository _repo;
    public ResetPasswordHandler(IPasswordResetRepository repo) => _repo = repo;

    public async Task<BaseResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.Token!));
        var tokenHash = Convert.ToHexString(hashBytes).ToLower();

        var record = await _repo.GetByHashAsync(tokenHash);
        if (record is null)
        {
            response.IsSuccess = false;
            response.Message = "Token inválido o expirado.";
            return response;
        }

        var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
        await _repo.UpdatePasswordAsync(record.UserId, newHash);
        await _repo.DeleteAsync(record.TokenId);

        response.IsSuccess = true;
        response.Message = "Contraseña actualizada correctamente.";
        response.Data = true;

        return response;
    }
}
