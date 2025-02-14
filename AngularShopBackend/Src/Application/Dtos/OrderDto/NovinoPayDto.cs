namespace Application.Dtos.OrderDto;

public class NovinoPayDto
{
    public string status { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
    public object data { get; set; }
    public Array errors { get; set; }
}
