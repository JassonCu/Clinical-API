using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Prescription.Commands.ChangeStateCommand
{
    public class ChangeStatePrescriptionHandler : IRequestHandler<ChangeStatePrescriptionCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChangeStatePrescriptionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(ChangeStatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var entity = _mapper.Map<Entity.Prescription>(request);
                var parameters = entity.GetPropertiesWithValues();
                response.Data = await _unitOfWork.Prescription.ExecAsync(StoreProcedures.uspPrescriptionChangeState, parameters);
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_UPDATE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
