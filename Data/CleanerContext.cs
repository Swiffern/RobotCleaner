using Microsoft.EntityFrameworkCore;
using RobotCleaner.Data.Models;

namespace RobotCleaner.Data
{
    public class CleanerContext : DbContext
    {
        public CleanerContext(DbContextOptions<CleanerContext> options) : base(options) { }

        public DbSet<CleaningResult> CleaningResult { get; set; }
    }
}
