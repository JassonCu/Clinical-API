using Clinical.Web.Core.DTOs.Doctor;

namespace Clinical.Web.Core.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorListDto>> GetAllAsync();
    Task<DoctorDetailDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateDoctorDto dto);
    Task<bool> UpdateAsync(UpdateDoctorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangeStateAsync(ChangeStateDoctorDto dto);
}
