using Application.Contracts;
using Application.Dtos.OrderDto;
using Application.Interfaces;
using Domain.Entities.BasketEntity;
using Domain.Entities.Order;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Features.Orders.Commands.Create;

public class CreateOrderCommand : IRequest<string>
{
    public string BasketId { get; set; }
    public int DeliveryMethodId { get; set; }
    public string BuyerPhoneNumber { get; set; }
    public PortalType PortalType { get; set; } = PortalType.Novino;
    public ShipToAddress ShipToAddress { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOWork _unitOWork;

    public CreateOrderCommandHandler(
        IBasketRepository basketRepository, IConfiguration configuration,
        ICurrentUserService currentUserService, IUnitOWork unitOWork)
    {
        _basketRepository = basketRepository;
        _configuration = configuration;
        _currentUserService = currentUserService;
        _unitOWork = unitOWork;
    }

    public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // دریافت سبد خرید و نحوه ارسال
        var basket = await _basketRepository.GetBasketAsync(request.BasketId);
        var deliveryMethod = await GetDeliveryMethod(request.DeliveryMethodId, cancellationToken);

        // محاسبه مبلغ
        var amount = (int)(basket.CalculateOriginalPrice() + deliveryMethod.Price);
        string callbackUrl = _configuration["Order:CallBack"];

        // ایجاد سفارش در دیتابیس
        var order = await CreateOrder(request, basket, deliveryMethod, amount, cancellationToken);

        // تولید شماره فاکتور و شناسه رهگیری پس از ذخیره شدن سفارش
        string invoiceNumber = GenerateInvoiceNumber(order.Id);
        string trackingCode = GenerateTrackingCode();

        // به روزرسانی سفارش با شماره فاکتور و شناسه رهگیری
        order.InvoiceNumber = invoiceNumber;
        order.TrackingCode = trackingCode;

        // ارسال درخواست پرداخت به درگاه و دریافت لینک پرداخت
        ResultNovinPay payment = await InitiatePayment(cancellationToken, callbackUrl, order);

        if (payment.payment_url != "false")
        {
            order.Authority = payment.authority;
            order.TransactionId = payment.trans_id;

            // ذخیره‌سازی تغییرات در یک مرحله
            await _unitOWork.SaveAsync(cancellationToken);

            // ارسال لینک پرداخت به فرانت‌اند
            return payment.payment_url;
        }
        else
        {
            throw new Exception("پرداخت به درستی راه اندازی نشد");
        }
    }

    public async Task<ResultNovinPay> InitiatePayment(
        CancellationToken cancellationToken,
        string callbackUrl,
        Order order
        )
    {
        string merchant = "test";
        string callbackMethod = "GET";

        var client = new RestClient(new RestClientOptions { Timeout = TimeSpan.FromMilliseconds(-1) });
        var request = new RestRequest("https://api.novinopay.com/payment/ipg/v2/request", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var body = new
        {
            merchant_id = merchant,
            amount = (order.SubTotal * 10),
            callback_url = callbackUrl,
            callback_method = callbackMethod,
            invoice_id = order.InvoiceNumber,
        };

        request.AddJsonBody(body);
        RestResponse response = await client.ExecuteAsync(request).ConfigureAwait(false);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            return null;
        }

        var novinoPay = JsonConvert.DeserializeObject<NovinoPayDto>(response.Content);
        if (novinoPay == null || string.IsNullOrEmpty(novinoPay.data?.ToString()))
        {
            return null;
        }

        var res = JsonConvert.DeserializeObject<ResultNovinPay>(novinoPay.data.ToString());
        return res;
    }

    private async Task<DeliveryMethod> GetDeliveryMethod(int deliveryMethodId, CancellationToken cancellationToken)
    {
        return await _unitOWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId, cancellationToken);
    }

    private async Task<Order> CreateOrder(
        CreateOrderCommand request,
        CustomerBasket basket,
        DeliveryMethod deliveryMethod,
        int amount,
        CancellationToken cancellationToken)
    {
        var orderItems = basket.Items.Select(item => new OrderItem
        {
            ItemOrdered = new ProductItemOrdered(item.Id, item.ProductTitle, item.Brand, item.Type, item.PictureUrl),
            Price = item.Price,
            Quantity = item.Quantity
        }).ToList();

        var order = new Order
        {
            BuyerPhoneNumber = request.BuyerPhoneNumber,
            ShipToAddress = request.ShipToAddress,
            DeliveryMethod = deliveryMethod,
            OrderItems = orderItems,
            SubTotal = basket.CalculateOriginalPrice() + (int)deliveryMethod.Price,
            PortalType = request.PortalType,
            CreatedBy = _currentUserService.UserId,
            OrderStatus = OrderStatus.Pending
        };

        await _unitOWork.Repository<Order>().AddAsync(order, cancellationToken);
        return order;
    }

    private string GenerateInvoiceNumber(int orderId)
    {
        return "INV-" + orderId.ToString("D6"); // INV-000001
    }

    private string GenerateTrackingCode()
    {
        return Guid.NewGuid().ToString("N");
    }
}


