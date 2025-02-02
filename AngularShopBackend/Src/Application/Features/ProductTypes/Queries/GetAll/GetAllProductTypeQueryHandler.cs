using Application.Contracts;
using Application.Dtos.ProductTypeDto;
using AutoMapper;
using Domain.Entities.ProductEntity;
using MediatR;

namespace Application.Features.ProductTypes.Queries.GetAll;

public class GetAllProductTypeQueryHandler : IRequestHandler<GetAllProductTypeQuery, IEnumerable<ProductTypeDto>>
{
    private readonly IUnitOWork _uow;
    private readonly IMapper _mapper;

    public GetAllProductTypeQueryHandler(IUnitOWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductTypeDto>> Handle(GetAllProductTypeQuery request,
        CancellationToken cancellationToken)
    {
        var result =  await _uow.Repository<ProductType>().GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProductTypeDto>>(result);
    }
}