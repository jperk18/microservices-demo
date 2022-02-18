﻿using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Patient.Domain.Console.Core;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Core.Pipelines;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;

namespace Health.Patient.Domain.Console.Commands.CreatePatientCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[PatientTransactionPipeline]
public sealed class CreatePatientAsyncCommandHandler : IAsyncCommandHandler<Console.Commands.CreatePatientCommand.CreatePatientCommand, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public CreatePatientAsyncCommandHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<PatientRecord> Handle(Console.Commands.CreatePatientCommand.CreatePatientCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Patients.Add(new Storage.Sql.Core.Databases.PatientDb.Models.Patient(
            Guid.NewGuid(), command.FirstName, command.LastName, command.DateOfBirth
        ));

        await _unitOfWork.Complete();
        return new PatientRecord(p.FirstName, p.LastName, p.DateOfBirth, p.Id);
    }
}