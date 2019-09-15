using Microsoft.EntityFrameworkCore;
using RobotCleaner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner.Data
{
    public class CleanerContext : DbContext
    {
        public CleanerContext(DbContextOptions<CleanerContext> options) : base(options) { }

        public DbSet<CleaningResult> CleaningResult { get; set; }
    }
}
