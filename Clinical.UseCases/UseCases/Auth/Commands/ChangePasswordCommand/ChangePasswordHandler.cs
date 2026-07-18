using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, BaseResponse<bool>>
    {
        private readonly IPasswordResetRepository _repo;
        public ChangePasswordHandler(IPasswordResetRepository repo) => _repo = repo;

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var hash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
                await _repo.UpdatePasswordAsync(request.UserId, hash);
                response.IsSuccess = true;
                response.Message = "Contraseña actualizada correctamente.";
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
