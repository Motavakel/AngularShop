using Application.Common.Mapping;
using AutoMapper;
using Domain.Entities.ProductEntity;


namespace Application.Dtos.ProductTypeDto;

public class ProductTypeDto :IMapFrom<ProductType>
{
    public int Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductType, ProductTypeDto>();
    }

}
