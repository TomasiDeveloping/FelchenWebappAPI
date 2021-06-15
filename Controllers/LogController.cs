using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class LogController : CustomBaseController
    {
        private readonly ILoggerService _loggerService;

        public LogController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LogDto>>> GetLogs()
        {
            var logs = await _loggerService.GetLogsAsync();
            if (logs.Count <= 0) return NoContent();
            return Ok(logs);
        }

        [HttpGet("{logId}")]
        public async Task<ActionResult<LogDto>> GetLogById(int logId)
        {
            var log = await _loggerService.GetLogByIdAsync(logId);
            if (log == null) return NotFound("Kein Log gefunden mit dieser Id");
            return Ok(log);
        }
    }
}