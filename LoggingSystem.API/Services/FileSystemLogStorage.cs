namespace LoggingSystem.API.Services
{
    public class FileSystemLogStorage : ILogStorage
    {

        private readonly string _logDirectory;

        public FileSystemLogStorage(string logDirectory)
        {
            _logDirectory = logDirectory;
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }



        public async Task<List<Log?>> GetLogsAsync(string? service, string? level, DateTime? start_time, DateTime? end_time)
        {
            var result = new List<Log>();

            // Validate the base log directory
            if (!Directory.Exists(_logDirectory))
                return result;

            // Build the service directory path
            var servicePath = string.IsNullOrEmpty(service) ? _logDirectory : Path.Combine(_logDirectory, service);

            if (!Directory.Exists(servicePath))
                return result;

            // Get all log files in the service directory
            var logFiles = Directory.GetFiles(servicePath, "*.log");

            foreach (var logFile in logFiles)
            {
                var logs = await File.ReadAllLinesAsync(logFile);

                foreach (var logLine in logs)
                {
                    // Assume logs are stored in a simple delimited format
                    var logParts = logLine.Split('|');
                    if (logParts.Length < 4) continue;

                    var logEntry = new Log
                    {
                        Timestamp = DateTime.Parse(logParts[0]),
                        Level = logParts[1],
                        Service = logParts[2],
                        Message = logParts[3]
                    };

                    // Apply filters
                    if (level != null && logEntry.Level != level) continue;
                    if (start_time.HasValue && logEntry.Timestamp < start_time.Value) continue;
                    if (end_time.HasValue && logEntry.Timestamp > end_time.Value) continue;

                    result.Add(logEntry);
                }
            }

            return result;
        }

        public async Task SaveLogAsync(Log logEntry)
        {
            var fileName = Path.Combine(_logDirectory, $"{logEntry.Service}_{DateTime.UtcNow:yyyyMMddHHmmss}.log");
            var logContent = $"{logEntry.Timestamp} [{logEntry.Level}] {logEntry.Service}: {logEntry.Message}";

            await File.WriteAllTextAsync(fileName, logContent);
        }
    }
}
