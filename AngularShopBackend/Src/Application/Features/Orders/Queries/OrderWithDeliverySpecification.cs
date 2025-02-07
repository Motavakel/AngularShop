using Application.Contracts.Specification;
using Domain.Entities.Order;

namespace Application.Features.Products.Queries.GetAll;

public class OrderWithDeliverySpecification : BaseSpecification<Order>
{
    public OrderWithDeliverySpecification(string authority)
        : base(x => x.Authority == authority)
    {
        AddInclude(x => x.DeliveryMethod);
    }
}