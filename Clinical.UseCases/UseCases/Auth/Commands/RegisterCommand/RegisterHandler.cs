using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Auth.Commands.RegisterCommand;

public class RegisterHandler : IRequestHandler<RegisterCommand, BaseResponse<bool>>
{
    private readonly IAuthRepository _authRepository;

    public RegisterHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<BaseResponse<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var existing = await _authRepository.GetUserByUsernameAsync(request.Username!);
        if (existing is not null)
            throw new ConflictException(GlobalMessage.MESSAGE_EXISTS);

        var user = new Entity.User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12),
            FirstName = request.FirstName,
            LastName = request.LastName,
            RoleId = request.RoleId
        };

        response.Data = await _authRepository.RegisterUserAsync(user);
        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_SAVE;
        }

        return response;
    }
}