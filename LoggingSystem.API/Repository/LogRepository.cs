using LoggingSystem.API.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LoggingSystem.API.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Log?> CreateAsync(Log logModel)
        {
            if (logModel is null)
            {
                return null;
            }

            await _context.Logs.AddAsync(logModel);
            await _context.SaveChangesAsync();

            return logModel;
        }

        public async Task<Log?> DeleteAsync(int id)
        {
            var logModel = await _context.Logs.FindAsync(id);

            if (logModel is null)
            {
                return null;
            }

            _context.Logs.Remove(logModel); 
            await _context.SaveChangesAsync();
            return logModel;
        }

        public async Task<IEnumerable<Log>> GetAllAsync(string? service, string? level, DateTime? start_time
            , DateTime? end_time, int pageNumber = 1, int pageSize = 1000)
        {
            var logsQuery = _context.Logs.AsQueryable();

            if (!string.IsNullOrEmpty(service))
                logsQuery = logsQuery.Where(log => log.Service == service);

            if (!string.IsNullOrEmpty(level))
                logsQuery = logsQuery.Where(log => log.Level == level);

            if (start_time.HasValue)
                logsQuery = logsQuery.Where(log => log.Timestamp >= start_time.Value);

            if (end_time.HasValue)
                logsQuery = logsQuery.Where(log => log.Timestamp <= end_time.Value);

            var logs = await logsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return logs;
        }

        public async Task<Log?> GetAsync(int id)
        {
            var log = await _context.Logs.FirstOrDefaultAsync(l => l.Id == id);

            if (log is null)
            {
                return null;
            }

            return log;
        }

        public async Task<Log?> UpdateAsync(int id, Log logModel)
        {
            var logDomain = await _context.Logs.FindAsync(id);

            if (logDomain is null)
            {
                return null;
            }

            logDomain.Service = logModel.Service;
            logDomain.Level = logModel.Level;
            logDomain.Timestamp = logModel.Timestamp;
            logDomain.Message = logModel.Message;   
            logDomain.BackendType = logModel.BackendType;

            await _context.SaveChangesAsync();
            return logDomain;
        }
    }
}
