using Application.Common.Mapping;
using AutoMapper;
using Domain.Entities.ProductEntity;


namespace Application.Dtos.ProductBrandDto;

public class ProductBrandDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, ProductBrandDto>();
    }
}
