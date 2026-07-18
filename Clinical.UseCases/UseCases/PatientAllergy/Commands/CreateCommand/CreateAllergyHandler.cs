using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.CreateCommand
{
    public class CreateAllergyHandler : IRequestHandler<CreateAllergyCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAllergyHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(CreateAllergyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var entity = _mapper.Map<Entity.PatientAllergy>(request);
                entity.State = 1;
                var parameters = entity.GetPropertiesWithValues();
                response.Data = await _unitOfWork.PatientAllergy.ExecAsync(StoreProcedures.uspAllergyRegister, parameters);
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
