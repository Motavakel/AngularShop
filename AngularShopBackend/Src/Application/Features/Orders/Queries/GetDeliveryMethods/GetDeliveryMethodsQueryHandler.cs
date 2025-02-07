using Application.Contracts;
using Domain.Entities.Order;
using MediatR;

namespace Application.Features.Orders.Queries.GetDeliveryMethods;

public class GetDeliveryMethodsQuery : IRequest<IReadOnlyList<DeliveryMethod>>
{
}

public class GetDeliveryMethodsQueryHandler : IRequestHandler<GetDeliveryMethodsQuery, IReadOnlyList<DeliveryMethod>>
{
    private readonly IUnitOWork _uow;

    public GetDeliveryMethodsQueryHandler(IUnitOWork uow)
    {
        _uow = uow;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        return await _uow.Repository<DeliveryMethod>().ToListAsync(cancellationToken);
    }
}