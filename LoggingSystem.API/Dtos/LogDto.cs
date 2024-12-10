using System.ComponentModel.DataAnnotations;

namespace LoggingSystem.API.Dtos
{
    public class LogDto
    {
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
