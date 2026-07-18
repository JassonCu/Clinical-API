using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Setup.Commands.SetupInitCommand
{
    public class SetupInitHandler : IRequestHandler<SetupInitCommand, BaseResponse<bool>>
    {
        private readonly ISetupRepository _repo;
        public SetupInitHandler(ISetupRepository repo) => _repo = repo;

        public async Task<BaseResponse<bool>> Handle(SetupInitCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                if (await _repo.IsInitializedAsync())
                {
                    response.IsSuccess = false;
                    response.Message = "El sistema ya fue inicializado.";
                    return response;
                }
                var hash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
                response.Data = await _repo.InitAsync(request.Username!, request.Email!, hash, request.FirstName!, request.LastName!);
                response.IsSuccess = response.Data;
                response.Message = response.Data ? "Sistema inicializado correctamente." : "Error al inicializar el sistema.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
