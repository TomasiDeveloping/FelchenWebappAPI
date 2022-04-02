using Api.Data;
using Api.Dtos;
using Api.Entities;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class LoggerService : ILoggerService
{
    private readonly FelchenContext _context;

    public LoggerService(IConfiguration configuration)
    {
        var options = new DbContextOptionsBuilder<FelchenContext>()
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")).Options;
        _context = new FelchenContext(options);
    }

    public async Task<List<LogDto>> GetLogsAsync()
    {
        return await _context.Logs
            .Include(l => l.LogType)
            .Select(l => new LogDto
            {
                Id = l.Id,
                Exception = l.ExceptionMessage,
                InnerException = l.InnerException,
                Message = l.Message,
                CreatedAt = l.CratedAt,
                LogType = l.LogType.Name
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<LogDto> GetLogByIdAsync(int logId)
    {
        return await _context.Logs
            .Include(l => l.LogType)
            .Select(l => new LogDto
            {
                Id = l.Id,
                Exception = l.ExceptionMessage,
                InnerException = l.InnerException,
                Message = l.Message,
                CreatedAt = l.CratedAt,
                LogType = l.LogType.Name
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == logId);
    }

    public async Task<Log> InsertLogAsync(Log log)
    {
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();
        return log;
    }
}