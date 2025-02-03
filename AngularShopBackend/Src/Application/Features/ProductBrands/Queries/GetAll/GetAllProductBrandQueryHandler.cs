using Application.Contracts;
using Application.Dtos.ProductBrandDto;
using AutoMapper;
using Domain.Entities.ProductEntity;
using MediatR;

namespace Application.Features.ProductBrands.Queries.GetAll;

public class GetAllProductBrandQueryHandler : IRequestHandler<GetAllProductBrandQuery, IEnumerable<ProductBrandDto>>
{
    private readonly IUnitOWork _uow;
    private readonly IMapper _mapper;

    public GetAllProductBrandQueryHandler(IUnitOWork uow,IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductBrandDto>> Handle(GetAllProductBrandQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ProductBrandSpec();
        var result = await _uow.Repository<ProductBrand>().ListAsyncSpec(spec, cancellationToken);
        return _mapper.Map<IEnumerable<ProductBrandDto>>(result);
    }
}