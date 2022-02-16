using Health.Nurse.Transports.Api.Models.Interfaces;

namespace Health.Nurse.Transports.Api.Models;

public class GetNurseApiRequest : INurseIdentifer
{
    public Guid Id { get; set; }
}