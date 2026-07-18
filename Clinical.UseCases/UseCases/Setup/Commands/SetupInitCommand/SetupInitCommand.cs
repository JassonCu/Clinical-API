using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Setup.Commands.SetupInitCommand
{
    public class SetupInitCommand : IRequest<BaseResponse<bool>>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
