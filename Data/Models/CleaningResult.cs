using System;

namespace RobotCleaner.Data.Models
{
    public class CleaningResult
    {
        public long id { get; set; }

        public DateTime timestamp { get; set; }

        public int commands { get; set; }

        public long result { get; set; }

        public double duration { get; set; }
    }
}
