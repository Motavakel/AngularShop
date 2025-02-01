using Application.Contracts;
using Application.Dtos.Products;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Products.Queries.GetAll;

public class GetAllProductsQuery : RequestParametersBasic, IRequest<PaginationResponse<ProductDto>>, ICacheQuery
{
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
}

