﻿using Microsoft.AspNetCore.Mvc;

namespace LoggingSystem.API.Interfaces
{
    public interface ILogRepository
    {
        Task<Log?> CreateAsync(Log logModel);
        Task<Log?> GetAsync(int id);
        Task<IEnumerable<Log>> GetAllAsync(string? service,string? level, DateTime? start_time,DateTime? end_time);   
        Task<Log?> DeleteAsync(int id);
        Task<Log?> UpdateAsync(int id,Log logModel);
    }
}