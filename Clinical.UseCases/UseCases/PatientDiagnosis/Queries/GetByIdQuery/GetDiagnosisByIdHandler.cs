using AutoMapper;
using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByIdQuery
{
    public class GetDiagnosisByIdHandler : IRequestHandler<GetDiagnosisByIdQuery, BaseResponse<GetAllDiagnosisResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiagnosisByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetAllDiagnosisResponseDto>> Handle(GetDiagnosisByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetAllDiagnosisResponseDto>();

            var entity = await _unitOfWork.PatientDiagnosis.GetByIdAsync(StoreProcedures.uspDiagnosisById, request);
            if (entity is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
            response.IsSuccess = true;
            response.Data = _mapper.Map<GetAllDiagnosisResponseDto>(entity);
            response.Message = GlobalMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
