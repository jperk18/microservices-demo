using Health.Patient.Domain.Storage.Sql.Databases.PatientDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Health.Patient.Domain.Utils.EfMigration;

static class Program
{
    static void Main(string[] args)
    {
        using IHost host = Console.Program.CreateHostBuilder(args).Build();

        System.Console.WriteLine("Starting: Patient database migration");
        try
        {
            using (var context = (PatientDbContext) host.Services.GetService(typeof(PatientDbContext))!)
            {
                context.Database.Migrate();
            }
            System.Console.WriteLine("Completed: Patient database migration");
        }
        catch (Exception e)
        {
            System.Console.WriteLine("ERROR: Patient database migration failed");
            System.Console.WriteLine(e);
            throw;
        }
        
    }
}