using Application.Dtos.Products;
using Application.Features.Products.Queries.Get;
using Application.Features.Products.Queries.GetAll;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;

namespace Store.tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new ProductsController();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(_mockMediator.Object);

        var httpContextMock = new DefaultHttpContext { RequestServices = serviceProviderMock.Object };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock };
    }

    [Fact]
    public async Task Get_Products_ReturnsPaginatedResult()
    {
        // Arrange
        var mockResponse = new PaginationResponse<ProductDto>(
            pageIndex: 1,
            pageSize: 12,
            count: 11,
            result: new List<ProductDto>
            {
            new ProductDto
            {
                Id = 11,
                Title = "دوچرخه دینو مدل 27.5",
                Price = 17800000,
                PictureUrl = "http://localhost:9001/images/products/im11.webp",
                ProductType = "شهری",
                ProductBrand = "المپیا",
                Description = "دوچرخه‌ها بهترین همراه ماجراجویی در خیابان‌های شهر و جاده‌های کوهستانی‌اند، بی‌نیاز از سوخت و پر از آزادی.",
                Summary = ""
            }
            }
        );

        // Act
        _mockMediator.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(mockResponse);
        var request = new GetAllProductsQuery();
        var result = await _controller.Get(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PaginationResponse<ProductDto>>(okResult.Value);
        Assert.Equal(1, returnValue.PageIndex);
        Assert.Equal(12, returnValue.PageSize);
        Assert.Equal(11, returnValue.Count);
        Assert.NotEmpty(returnValue.Result);
        Assert.Equal("دوچرخه دینو مدل 27.5", returnValue.Result.First().Title);
    }

    [Fact]
    public async Task Get_Product()
    {
        // Arrange
        var productResponse = new ProductDto
        {
            Id = 11,
            Title = "دوچرخه دینو مدل 27.5",
            Price = 17800000,
            PictureUrl = "http://localhost:9001/images/products/im11.webp",
            ProductType = "شهری",
            ProductBrand = "المپیا",
            Description = "دوچرخه‌ها بهترین همراه ماجراجویی در خیابان‌های شهر و جاده‌های کوهستانی‌اند، بی‌نیاز از سوخت و پر از آزادی.",
            Summary = ""
        };

        // Act
        _mockMediator.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(productResponse);

        var request = 11 ;
        var result = await _controller.Get(request, CancellationToken.None);

        // Asser
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(productResponse.Id, returnValue.Id);
        Assert.Equal(productResponse.Title, returnValue.Title);
        Assert.Equal(productResponse.Price, returnValue.Price);
        Assert.Equal(productResponse.PictureUrl, returnValue.PictureUrl);
        Assert.Equal(productResponse.ProductType, returnValue.ProductType);
        Assert.Equal(productResponse.ProductBrand, returnValue.ProductBrand);
        Assert.Equal(productResponse.Description, returnValue.Description);
        Assert.Equal(productResponse.Summary, returnValue.Summary);

    }
}
