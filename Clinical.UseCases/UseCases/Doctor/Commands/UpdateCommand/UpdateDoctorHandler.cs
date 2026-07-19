using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;

public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = _mapper.Map<Entity.Doctor>(request);
        var parameters = doctor.GetPropertiesWithValues();

        if (!await _unitOfWork.Doctor.ExecAsync(StoreProcedures.uspDoctorEdit, parameters))
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
    }
}
