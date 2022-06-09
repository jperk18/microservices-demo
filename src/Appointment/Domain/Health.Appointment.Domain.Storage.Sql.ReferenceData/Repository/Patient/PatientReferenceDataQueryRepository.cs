﻿using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Generic;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

public class PatientReferenceDataQueryRepository : GenericReferenceDataQueryRepository<ReferenceData.Database.Entities.Patient>, IPatientReferenceDataQueryRepository
{
#pragma warning disable CS0108, CS0114
    private readonly ReferenceDataDbContext _context;
#pragma warning restore CS0108, CS0114

    public PatientReferenceDataQueryRepository(ReferenceDataDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}