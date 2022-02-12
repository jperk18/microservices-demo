﻿namespace Health.Patient.Domain.Console.Queries.Core;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery command);
}