using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Commands.ChangeStateCommand
{
    public class ChangeStatePrescriptionCommand : IRequest<BaseResponse<bool>>
    {
        public int? PrescriptionId { get; set; }
        public int? State { get; set; }
    }
}
