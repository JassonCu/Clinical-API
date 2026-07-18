using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.VitalSign.Commands.CreateCommand
{
    public class CreateVitalSignHandler : IRequestHandler<CreateVitalSignCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVitalSignHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(CreateVitalSignCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var entity = _mapper.Map<Entity.VitalSign>(request);

            if (entity.Weight.HasValue && entity.Height.HasValue && entity.Height > 0)
                entity.Bmi = Math.Round(entity.Weight.Value / (decimal)Math.Pow((double)(entity.Height.Value / 100), 2), 2);

            entity.State = 1;
            var parameters = entity.GetPropertiesWithValues();
            response.Data = await _unitOfWork.VitalSign.ExecAsync(StoreProcedures.uspVitalSignRegister, parameters);
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }

            return response;
        }
    }
}
