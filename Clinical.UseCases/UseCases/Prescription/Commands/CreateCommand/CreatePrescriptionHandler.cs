using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand
{
    public class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePrescriptionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var entity = _mapper.Map<Entity.Prescription>(request);
                entity.State = 1;
                var parameters = entity.GetPropertiesWithValues();
                response.Data = await _unitOfWork.Prescription.ExecAsync(StoreProcedures.uspPrescriptionRegister, parameters);
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
