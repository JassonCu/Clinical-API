using Clinical.Application.DTOS.User.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.User.Queries.GetByIdQuery
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDetailDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<UserDetailDto>();

            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

            response.IsSuccess = true;
            response.Data = user;
            response.Message = GlobalMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
