From\
 /src/Patient/Domain/Health.Patient.Domain.Storage.Sql\
\
Migrations Run:\
dotnet ef migrations add {name} -s ../Health.Patient.Domain.Console

Update database Run:\
dotnet ef database update -s ../Health.Patient.Domain.Console

\
\
Additional Notes:
\
Run migrations from container (domain util):\
docker exec -it health-patientdomain-1 /migrations/migrate