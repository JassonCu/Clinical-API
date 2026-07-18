using Clinical.Application.DTOS.User.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Queries.GetAllQuery
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<IEnumerable<UserListDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<IEnumerable<UserListDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<UserListDto>>();

            var users = await _userRepository.GetAllUsersAsync();

            if (users is not null)
            {
                response.IsSuccess = true;
                response.Data = users;
                response.Message = GlobalMessage.MESSAGE_QUERY;
            }

            return response;
        }
    }
}
