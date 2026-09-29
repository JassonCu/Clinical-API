using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.Utils.Constants;
using Microsoft.Extensions.Configuration;

namespace Clinical.UseCases.Services
{
    public class AppointmentConflictChecker : IAppointmentConflictChecker
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeSpan _slot;

        public AppointmentConflictChecker(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            var minutes = int.TryParse(configuration["Appointment:SlotMinutes"], out var m) && m > 0 ? m : 30;
            _slot = TimeSpan.FromMinutes(minutes);
        }

        public async Task<Appointment?> FindConflictAsync(int doctorId, DateTime when, int? excludeAppointmentId = null)
        {
            var appointments = await _unitOfWork.Appointment.GetAllAsync(
                StoreProcedures.uspAppointmentByDoctor, new { DoctorId = doctorId });

            return appointments.FirstOrDefault(a =>
                a.State == 1
                && a.AppointmentId != excludeAppointmentId
                && a.AppointmentDate.HasValue
                && Math.Abs((a.AppointmentDate.Value - when).TotalMinutes) < _slot.TotalMinutes);
        }
    }
}
