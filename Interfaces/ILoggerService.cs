using Api.Dtos;
using Api.Entities;

namespace Api.Interfaces;

public interface ILoggerService
{
    public Task<List<LogDto>> GetLogsAsync();
    public Task<LogDto> GetLogByIdAsync(int logId);
    public Task<Log> InsertLogAsync(Log log);
}