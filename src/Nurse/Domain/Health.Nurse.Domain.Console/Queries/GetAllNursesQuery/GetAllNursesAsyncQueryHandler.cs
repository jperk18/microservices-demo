﻿using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Console.Queries.Core;
using Health.Nurse.Domain.Storage.Sql;

namespace Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllNursesAsyncQueryHandler : IAsyncQueryHandler<GetAllNursesQuery, IEnumerable<NurseRecord>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllNursesAsyncQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<NurseRecord>> Handle(GetAllNursesQuery command)
    {
        return await Task.FromResult(
            _unitOfWork.Nurses.GetAll().Select(i => new NurseRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id))
            );
    }
}