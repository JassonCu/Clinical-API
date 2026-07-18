using FluentValidation;

namespace Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("El campo PacienteId debe ser mayor a 0.");

        RuleFor(x => x.DoctorId)
            .GreaterThan(0).WithMessage("El campo MédicoId debe ser mayor a 0.");

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.Now).WithMessage("La fecha de la cita debe ser posterior a la fecha actual.");

        RuleFor(x => x.Reason)
            .NotNull().WithMessage("El motivo de la cita no puede ser nulo.")
            .NotEmpty().WithMessage("El motivo de la cita no puede ser vacío.")
            .MaximumLength(500).WithMessage("El motivo no puede superar 500 caracteres.");
    }
}
