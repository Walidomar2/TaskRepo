namespace LoggingSystem.API.Interfaces
{
    public interface ILogStorage
    {
        Task SaveLogAsync(Log logEntry);
        Task<List<Log?>> GetLogsAsync(string? service, string? level, DateTime? start_time, DateTime? end_time);
    }
}
