using System.Linq.Expressions;
using Health.Shared.Application.Core;
using Health.Shared.Domain.Exceptions.Models;

namespace Health.Appointment.Domain.Console.Core.Exceptions;

public static class AppointmentDomainExceptions
{
    public static AppointmentDomainValidationException AppointmentNotExist<TModel, TProperty>(Guid Appointment, Expression<Func<TModel, TProperty>> property)
    {
        return new AppointmentDomainValidationException(
            string.Format(AppointmentDomainExceptionMessages.AppointmentNotExist, Appointment),
            new List<DomainValidationFailureDto>()
            {
                new(nameof(AppointmentDomainExceptionMessages.AppointmentNotExist),
                    AppointmentDomainExceptionMessages.AppointmentNotExist)
                {
                    AttemptedValue = Appointment,
                    PropertyName = Reflection.GetPropertyName(property),
                    ErrorCode = nameof(AppointmentNotExist)
                }
            });
    }
    
    public static AppointmentDomainValidationException PatientNotFound<TModel, TProperty>(Guid Patient, Expression<Func<TModel, TProperty>> property)
    {
        return new AppointmentDomainValidationException(
            string.Format(AppointmentDomainExceptionMessages.AppointmentNotExist, Patient),
            new List<DomainValidationFailureDto>()
            {
                new(nameof(AppointmentDomainExceptionMessages.PatientNotFound),
                    AppointmentDomainExceptionMessages.PatientNotFound)
                {
                    AttemptedValue = Patient,
                    PropertyName = Reflection.GetPropertyName(property),
                    ErrorCode = nameof(PatientNotFound)
                }
            });
    }
    
    public static AppointmentDomainValidationException NurseNotFound<TModel, TProperty>(Guid Nurse, Expression<Func<TModel, TProperty>> property)
    {
        return new AppointmentDomainValidationException(
            string.Format(AppointmentDomainExceptionMessages.AppointmentNotExist, Nurse),
            new List<DomainValidationFailureDto>()
            {
                new(nameof(AppointmentDomainExceptionMessages.NurseNotFound),
                    AppointmentDomainExceptionMessages.NurseNotFound)
                {
                    AttemptedValue = Nurse,
                    PropertyName = Reflection.GetPropertyName(property),
                    ErrorCode = nameof(NurseNotFound)
                }
            });
    }
    
    public static AppointmentDomainValidationException ScheduledAppointmentNotFound<TModel, TProperty>(Guid Appointment, Expression<Func<TModel, TProperty>> property)
    {
        return new AppointmentDomainValidationException(
            string.Format(AppointmentDomainExceptionMessages.ScheduledAppointmentNotFound, Appointment),
            new List<DomainValidationFailureDto>()
            {
                new(nameof(AppointmentDomainExceptionMessages.ScheduledAppointmentNotFound),
                    AppointmentDomainExceptionMessages.NurseNotFound)
                {
                    AttemptedValue = Appointment,
                    PropertyName = Reflection.GetPropertyName(property),
                    ErrorCode = nameof(ScheduledAppointmentNotFound)
                }
            });
    }
}