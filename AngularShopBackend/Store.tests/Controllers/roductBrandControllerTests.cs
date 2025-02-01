using Application.Features.ProductBrands.Queries.GetAll;
using Domain.Entities.ProductEntity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using FluentAssertions;


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
        serviceProviderMock.Setup(sp => sp.GetService(typeof(ISender))).Returns(_mediatorMock.Object);

        var httpContextMock = new DefaultHttpContext { RequestServices = serviceProviderMock.Object };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock };
    }

    [Fact]
    public async Task Get_ShouldReturnListOfProductBrands_WhenCalled()
    {
        //ایجاد لیست پاسخ
        var brands = new List<ProductBrand>
        {
            new ProductBrand { Id = 1, Title = "BrandA" },
            new ProductBrand { Id = 2, Title = "BrandB" }
        };


        //در اینجا هم ارسال در خواست به سرویس را توسط مدیاتور شبیه سازی میشه
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllProductBrandQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(brands);

        var result = await _controller.Get(CancellationToken.None);
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductBrand>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        

        var returnedBrands = Assert.IsType<List<ProductBrand>>(okResult.Value);

        returnedBrands.Should().HaveCount(2);
        returnedBrands.Should().Contain(b => b.Title == "BrandA");
        returnedBrands.Should().Contain(b => b.Title == "BrandB");
    }
}
