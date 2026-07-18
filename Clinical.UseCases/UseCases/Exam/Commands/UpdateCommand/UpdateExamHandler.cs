using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Exam.Commands.UpdateCommand;

public class UpdateExamHandler : IRequestHandler<UpdateExamCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExamHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var exam = _mapper.Map<Entity.Exam>(request);
            var parameters = exam.GetPropertiesWithValues();
            response.Data = await _unitOfWork.Exam.ExecAsync(StoreProcedures.uspExamEdit, parameters);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = GlobalMessage.MESSAGE_UPDATE;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}