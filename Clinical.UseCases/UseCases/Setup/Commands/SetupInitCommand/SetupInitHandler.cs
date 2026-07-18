using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
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

            if (await _repo.IsInitializedAsync())
                throw new ConflictException("El sistema ya fue inicializado.");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
            response.Data = await _repo.InitAsync(request.Username!, request.Email!, hash, request.FirstName!, request.LastName!);
            response.IsSuccess = response.Data;
            response.Message = response.Data ? "Sistema inicializado correctamente." : "Error al inicializar el sistema.";

            return response;
        }
    }
}
