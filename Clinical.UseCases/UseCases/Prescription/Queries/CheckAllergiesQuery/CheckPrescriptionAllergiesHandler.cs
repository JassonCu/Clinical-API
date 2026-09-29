using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.CheckAllergiesQuery
{
    public class CheckPrescriptionAllergiesHandler
        : IRequestHandler<CheckPrescriptionAllergiesQuery, BaseResponse<AllergyCheckResponseDto>>
    {
        private readonly IPrescriptionAllergyService _allergyService;

        public CheckPrescriptionAllergiesHandler(IPrescriptionAllergyService allergyService)
            => _allergyService = allergyService;

        public async Task<BaseResponse<AllergyCheckResponseDto>> Handle(
            CheckPrescriptionAllergiesQuery request, CancellationToken cancellationToken)
        {
            var conflicts = await _allergyService.FindConflictsAsync(request.PatientId, request.MedicineIds);

            return new BaseResponse<AllergyCheckResponseDto>
            {
                IsSuccess = true,
                Message = GlobalMessage.MESSAGE_QUERY,
                Data = new AllergyCheckResponseDto
                {
                    HasConflicts = conflicts.Count > 0,
                    Conflicts = conflicts
                }
            };
        }
    }
}
