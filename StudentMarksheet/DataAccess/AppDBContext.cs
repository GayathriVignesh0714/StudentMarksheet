using Microsoft.EntityFrameworkCore;
using System.Configuration;
using StudentMarksheet.Model;

namespace StudentMarksheet.DataAccess
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : 
            base(options) { }

        public DbSet<StudentMarksEntity> StudentMarks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 🔹 Read connection string from App.config
                string connectionString = ConfigurationManager.ConnectionStrings["StudentMarksDB"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Missing 'StudentMarksDB' connection string in App.config.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
