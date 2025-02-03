using Application.Dtos.ProductBrandDto;
using MediatR;

namespace Application.Features.ProductBrands.Queries.GetAll;

public class GetAllProductBrandQuery : IRequest<IEnumerable<ProductBrandDto>>
{
}