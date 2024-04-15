using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Analysis.Commands.UpdateCommand
{
    public class UpdateAnalysisHandler : IRequestHandler<UpdateAnalysisCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAnalysisHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(UpdateAnalysisCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var analysis = _mapper.Map<Entity.Analysis>(request);
                var parameters = analysis.GetPropertiesWithValues();
                response.Data = await _unitOfWork.Analysis.ExecAsync(StoreProcedures.uspAnalysisEdit, parameters);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = "Actualización Exitosa";
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
}
