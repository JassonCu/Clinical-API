using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Commands.DeleteCommand
{
    public class DeleteVitalSignCommand : IRequest<BaseResponse<bool>>
    {
        public int VitalSignId { get; set; }
    }
}
