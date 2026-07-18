using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Patient.Commands.CreateCommand;

public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var patient = _mapper.Map<Entity.Patient>(request);
        var parameters = patient.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Patient.ExecAsync(StoreProcedures.uspPatientRegister, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_SAVE;
        }

        return response;
    }
}
