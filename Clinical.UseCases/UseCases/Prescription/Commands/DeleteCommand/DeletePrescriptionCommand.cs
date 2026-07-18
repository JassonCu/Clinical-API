using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Commands.DeleteCommand
{
    public class DeletePrescriptionCommand : IRequest<BaseResponse<bool>>
    {
        public int PrescriptionId { get; set; }
    }
}
