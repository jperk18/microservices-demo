using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Generic;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Nurse;

public class NurseReferenceDataRepository : GenericReferenceDataRepository<ReferenceData.Database.Entities.Nurse>, INurseReferenceDataRepository
{
#pragma warning disable CS0108, CS0114
    private readonly ReferenceDataDbContext _context;
#pragma warning restore CS0108, CS0114

    public NurseReferenceDataRepository(ReferenceDataDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}