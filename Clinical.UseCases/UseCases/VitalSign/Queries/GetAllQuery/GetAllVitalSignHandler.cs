using AutoMapper;
using Clinical.Application.DTOS.VitalSign.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Queries.GetAllQuery
{
    public class GetAllVitalSignHandler : IRequestHandler<GetAllVitalSignQuery, BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVitalSignHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>> Handle(GetAllVitalSignQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>();
            try
            {
                var entities = await _unitOfWork.VitalSign.GetAllAsync(StoreProcedures.uspVitalSignList);
                if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllVitalSignResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
