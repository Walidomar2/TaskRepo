using System.ComponentModel.DataAnnotations;

namespace LoggingSystem.API.Dtos
{
    public class UpdateLogDto
    {
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
