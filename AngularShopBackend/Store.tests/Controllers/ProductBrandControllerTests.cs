using Application.Features.ProductBrands.Queries.GetAll;
using Domain.Entities.ProductEntity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using FluentAssertions;
using Application.Dtos.ProductBrandDto;


namespace Store.tests.Controllers;

public class ProductBrandControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductBrandController _controller;

    public ProductBrandControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductBrandController();

      
        var serviceProviderMock = new Mock<IServiceProvider>();
        
       
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(_mediatorMock.Object);

        var httpContextMock = new DefaultHttpContext { RequestServices = serviceProviderMock.Object };

        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock };
    }

    [Fact]
    public async Task Get_ShouldReturnListOfProductBrands_WhenCalled()
    {
        //ایجاد لیست پاسخ
        var brands = new List<ProductBrandDto>
        {
            new ProductBrandDto { Id = 1, Title = "لاودیس" },
            new ProductBrandDto { Id = 2, Title = "جیمز" },
            new ProductBrandDto { Id = 2, Title = "المپیا" }
        };


        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllProductBrandQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(brands);

        var result = await _controller.Get(CancellationToken.None);
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductBrandDto>>>(result);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        

        var returnedBrands = Assert.IsType<List<ProductBrandDto>>(okResult.Value);

        returnedBrands.Should().HaveCount(3);
        returnedBrands.Should().Contain(b => b.Title == "لاودیس");
        returnedBrands.Should().Contain(b => b.Title == "جیمز");
        returnedBrands.Should().Contain(b => b.Title == "المپیا");
    }
}
