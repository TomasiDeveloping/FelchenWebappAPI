using System;

namespace Api.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime CratedAt { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
        public int LogTypeId { get; set; }
        public LogType LogType { get; set; }
    }
}