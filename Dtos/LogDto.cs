namespace Api.Dtos;

public class LogDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
    public string Exception { get; set; }
    public string InnerException { get; set; }
    public string LogType { get; set; }
}