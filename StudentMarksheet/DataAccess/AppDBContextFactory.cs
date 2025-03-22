using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace StudentMarksheet.DataAccess
{
    public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();

            // 🔹 Read from App.config manually
            string connectionString = ConfigurationManager.ConnectionStrings["StudentMarksDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Missing 'StudentMarksDB' connection string in App.config.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDBContext(optionsBuilder.Options);
        }
    }
}
