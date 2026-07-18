using Clinical.Application.DTOS.User.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Queries.GetRolesQuery
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, BaseResponse<IEnumerable<RoleDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllRolesHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<RoleDto>>();

            var roles = await _userRepository.GetAllRolesAsync();

            if (roles is not null)
            {
                response.IsSuccess = true;
                response.Data = roles;
                response.Message = GlobalMessage.MESSAGE_QUERY;
            }

            return response;
        }
    }
}
