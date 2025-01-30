namespace Domain.Exceptions;

public class ApiToReturn
{
    public ApiToReturn(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
        Messages = new List<string> { message };
    }

    public ApiToReturn(int statusCode, List<string> messages)
    {
        StatusCode = statusCode;
        Messages = messages ?? new List<string>();
    }

    public string Message { get; set; }
    public int StatusCode { get; set; }
    public List<string> Messages { get; set; } = new();
}
