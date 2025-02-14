using Domain.Entities.Base;
using Domain.Entities.Identity;
using Domain.Enums;

namespace Domain.Entities.Order;

public class Order : BaseAuditableEntity
{

    public string BuyerPhoneNumber { get; set; }
    public string InvoiceNumber { get; set; }
    public int SubTotal { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string Authority { get; set; }
    public string TrackingCode { get; set; }
    public long TransactionId { get; set; }


    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public PortalType PortalType { get; set; } = PortalType.Novino;
    public bool IsFinally { get; set; } = false; 


    public List<OrderItem> OrderItems { get; set; } = new();
    public ShipToAddress ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public User User { get; set; }
}