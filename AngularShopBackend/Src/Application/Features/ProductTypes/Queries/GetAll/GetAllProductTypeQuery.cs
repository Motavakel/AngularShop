using Application.Dtos.ProductTypeDto;
using MediatR;

namespace Application.Features.ProductTypes.Queries.GetAll;

public class GetAllProductTypeQuery : IRequest<IEnumerable<ProductTypeDto>>
{

}