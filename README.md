From\
 /src/Patient/Domain/Health.Patient.Domain.Storage.Sql\
\
Run:\
dotnet ef database update -s ../Health.Patient.Domain.Console

Run migrations from container (domain util):\
docker exec -it health-patientdomain-1 /migrations/migrate