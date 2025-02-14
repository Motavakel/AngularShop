using Application.Contracts.Specification;
using Domain.Entities.Order;

namespace Application.Features.Orders.Commands;

public class OrderByAuthoritySpecification: BaseSpecification<Order>
{
    public OrderByAuthoritySpecification(string authority):base(x => x.Authority == authority)
    {

    }
}
