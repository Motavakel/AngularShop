using Application.Contracts;
using Application.Dtos.OrderDto;
using AutoMapper;
using Domain.Entities.Order;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Features.Orders.Commands.Verify;

public class VerifyCommand : IRequest<OrderDto>
{
    public string Authority { get; set; }
    public string PaymentStatus { get; set; }
    public string invoiceID { get; set; }

    public VerifyCommand(string authority, string InvoiceID, string paymentStatus)
    {
        Authority = authority;
        PaymentStatus = paymentStatus;
        invoiceID = InvoiceID;
    }
}

public class VerifyCommandHandler : IRequestHandler<VerifyCommand,OrderDto>
{
    private readonly IUnitOWork _uow;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public VerifyCommandHandler(
        IUnitOWork uow,
        IConfiguration configuration,
        IMapper mapper,
        ILogger<VerifyCommandHandler> logger) 
    {
        _uow = uow;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(
        VerifyCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new OrderByAuthoritySpecification(request.Authority);
        var order = await _uow.Repository<Order>().GetEntityWithSpec(spec, cancellationToken);
        int amount = order.SubTotal * 10;

        if (order == null)
        {
            return new OrderDto
            {
                OrderStatus = OrderStatus.PaymentFailed,
            };
        }

        var paymentResult = await CheckPayment(amount, request.Authority);
        if (request.PaymentStatus == "NOK" || !paymentResult.StartsWith("پرداخت موفق"))
        {

            order.PaymentDate = DateTime.UtcNow;
            order.OrderStatus = OrderStatus.PaymentFailed;
            await _uow.SaveAsync(cancellationToken);

            return new OrderDto
            {
                OrderStatus = OrderStatus.PaymentFailed,
                PaymentDate = DateTime.UtcNow,
            };
        }

        order.PaymentDate = DateTime.UtcNow;
        order.OrderStatus = OrderStatus.PaymentSuccess;
        await _uow.SaveAsync(cancellationToken);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<string> CheckPayment(int amount, string authority)
    {
        try
        {
            string merchant = "test";

            var client = new RestClient(new RestClientOptions { Timeout = TimeSpan.FromSeconds(30) });
            var request = new RestRequest("https://api.novinopay.com/payment/ipg/v2/verification", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var body = new
            {
                merchant_id = merchant,
                amount = amount,
                authority = authority
            };

            request.AddJsonBody(body);

            RestResponse response = await client.ExecuteAsync(request).ConfigureAwait(false);


            if (!response.IsSuccessStatusCode)
            {
                return $"خطا در برقراری ارتباط با درگاه: {response.StatusCode} - {response.ErrorMessage}";
            }

            var result = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);

            if (result == null || result.status != "100")
            {
                return $"پرداخت تأیید نشد: {result?.message ?? "پاسخ نامعتبر از درگاه"}";
            }

            return $"پرداخت موفق - شماره پیگیری: {result.data.ref_id}";
        }
        catch (Exception ex)
        {
            return $"خطای سیستمی رخ داده: {ex.Message}";
        }
    }
}



public class PaymentResponse
{
    public string status { get; set; }
    public string message { get; set; }
    public PaymentData data { get; set; }
}

public class PaymentData
{
    public long trans_id { get; set; }
    public string ref_id { get; set; }
    public string authority { get; set; }
    public string card_pan { get; set; }
    public int amount { get; set; }
    public string buyer_ip { get; set; }
    public long payment_time { get; set; }
}
