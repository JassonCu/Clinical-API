using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Setup.Queries.GetSetupStatusQuery
{
    public class GetSetupStatusHandler : IRequestHandler<GetSetupStatusQuery, BaseResponse<bool>>
    {
        private readonly ISetupRepository _repo;
        public GetSetupStatusHandler(ISetupRepository repo) => _repo = repo;

        public async Task<BaseResponse<bool>> Handle(GetSetupStatusQuery request, CancellationToken cancellationToken)
        {
            var initialized = await _repo.IsInitializedAsync();
            return new BaseResponse<bool> { IsSuccess = true, Data = initialized };
        }
    }
}
