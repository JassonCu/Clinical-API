using Clinical.Application.DTOS.User.Request;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.UpdateCommand
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var dto = new UpdateUserDto
                {
                    UserId    = request.UserId,
                    FirstName = request.FirstName,
                    LastName  = request.LastName,
                    Email     = request.Email,
                    RoleId    = request.RoleId
                };

                await _userRepository.UpdateUserAsync(dto);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = GlobalMessage.MESSAGE_UPDATE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
