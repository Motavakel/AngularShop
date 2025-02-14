using Application.Common.Mapping;
using Application.Dtos.Common;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Entities.Order;
using Domain.Enums;

namespace Application.Dtos.OrderDto;

public class OrderDto : BaseAuditableEntityDtoResponse, IMapFrom<Order>
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
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.ShipToAddress, opt => opt.MapFrom(src => src.ShipToAddress))
            .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }

}
