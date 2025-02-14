namespace Application.Dtos.OrderDto;

public class ResultNovinPay
{
    public int wage { get; set; }
    public int trans_id { get; set; }
    public string wage_payer { get; set; } = string.Empty;
    public string authority { get; set; } = string.Empty;
    public string payment_url { get; set; } = string.Empty;
}