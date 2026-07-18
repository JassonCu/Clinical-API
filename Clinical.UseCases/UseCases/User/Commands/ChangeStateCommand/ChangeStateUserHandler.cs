using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.ChangeStateCommand
{
    public class ChangeStateUserHandler : IRequestHandler<ChangeStateUserCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepository;

        public ChangeStateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Handle(ChangeStateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            await _userRepository.ChangeUserStateAsync(request.UserId, request.State);

            response.IsSuccess = true;
            response.Data = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;

            return response;
        }
    }
}
