using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, BaseResponse<bool>>
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordResetRepository _repo;

    public ChangePasswordHandler(IAuthRepository authRepository, IPasswordResetRepository repo)
    {
        _authRepository = authRepository;
        _repo = repo;
    }

    public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        // Identity comes from the token (request.Username), never from the client body.
        var user = await _authRepository.GetUserByUsernameAsync(request.Username);

        if (user is null || string.IsNullOrEmpty(user.PasswordHash) ||
            !BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            response.IsSuccess = false;
            response.Message = "La contraseña actual es incorrecta.";
            return response;
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
        // Update targets the id resolved from the authenticated record, not any client-supplied value.
        await _repo.UpdatePasswordAsync(user.UserId!.Value, hash);

        response.IsSuccess = true;
        response.Data = true;
        response.Message = "Contraseña actualizada correctamente.";
        return response;
    }
}
