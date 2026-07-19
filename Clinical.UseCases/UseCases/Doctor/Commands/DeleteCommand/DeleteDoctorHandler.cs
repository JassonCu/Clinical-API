using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;

public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDoctorHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Doctor.ExecAsync(StoreProcedures.uspDoctorRemove, request))
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
    }
}
