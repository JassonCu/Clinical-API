using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.User.Commands.UpdateCommand
{
    public class UpdateUserCommand : IRequest<BaseResponse<bool>>
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
    }
}
