using Application.Contracts.Specification;
using Application.Wrappers;
using Domain.Entities.ProductEntity;
using System.Linq.Expressions;

namespace Application.Features.Products.Queries.GetAll;

public class GetProductSpec : BaseSpecification<Product>
{
    public GetProductSpec(GetAllProductsQuery specParams) : base(Expression.ExpressionSpec(specParams))
    {
        //include

        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);

        //sort
        switch (specParams.TypeSort)
        {
            case SortOptions.Newest:
                AddOrderByDesc(x => x.Id); 
                break;
            case SortOptions.PriceHighToLow:
                AddOrderByDesc(x => x.Price);
                break;
            case SortOptions.PriceLowToHigh:
                AddOrderBy(x => x.Price);
                break;
            case SortOptions.NameAToZ:
                AddOrderBy(x => x.Title);
                break;
            default:
                AddOrderByDesc(x => x.Id);
                break;
        }

        //pagination
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize, true);
    }
    public GetProductSpec(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);
    }
}

public class ProductsCountSpec : BaseSpecification<Product>
{
    public ProductsCountSpec(GetAllProductsQuery specParams) : base(Expression.ExpressionSpec(specParams))
    {
        IsPagingEnabled = false;
    }
}



//با توجه به اینکه در بخش بیس فراخوانی میشه، باید ابتدا مقدار این بخش دریافت وبعد به سازنده والد موردنظر منتقل شود
//پس  به محض ورود این فیلتر اعمال می شود، تا براساس نتیجه فیلتر محصولات درسایر جزئیات هم بروز شوند 
public class Expression
{
    public static Expression<Func<Product, bool>> ExpressionSpec(GetAllProductsQuery specParams)
    {
        return x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Title.ToLower().Contains(specParams.Search))
            &&
            (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId.Value)
            &&
            (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId.Value);
    }
}
