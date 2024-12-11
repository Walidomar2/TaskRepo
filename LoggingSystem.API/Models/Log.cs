namespace LoggingSystem.API.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        public string Service { get; set; }

        [Required]
        [RegularExpression("(info|warning|error)")]
        public string Level { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string BackendType { get; set; }
    }
}
