using AutoMapper;
using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByIdQuery
{
    public class GetAllergyByIdHandler : IRequestHandler<GetAllergyByIdQuery, BaseResponse<GetAllAllergyResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllergyByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetAllAllergyResponseDto>> Handle(GetAllergyByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetAllAllergyResponseDto>();

            var entity = await _unitOfWork.PatientAllergy.GetByIdAsync(StoreProcedures.uspAllergyById, request);
            if (entity is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
            response.IsSuccess = true;
            response.Data = _mapper.Map<GetAllAllergyResponseDto>(entity);
            response.Message = GlobalMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
