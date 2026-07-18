using FluentValidation;

namespace Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("El campo CitaId debe ser mayor a 0.");

            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.Now).WithMessage("La fecha de la cita debe ser posterior a la fecha actual.")
                .When(x => x.AppointmentDate.HasValue);

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("El motivo de la cita no puede ser vacío.")
                .MaximumLength(500).WithMessage("El motivo no puede superar 500 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Reason));
        }
    }
}
