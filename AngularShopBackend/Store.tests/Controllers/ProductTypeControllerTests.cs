using Application.Dtos.ProductTypeDto;
using Application.Features.ProductTypes.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;

namespace Store.tests.Controllers;

public class ProductTypeControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProductTypeController _controller;

    public ProductTypeControllerTests()
    {
        _mockMediator = new();
        _controller = new();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(_mockMediator.Object);
        var httpContextMock = new DefaultHttpContext { RequestServices = serviceProviderMock.Object };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContextMock };
    }

    [Fact]
    public async Task Get_Product_Type_Should_Return_Expected_ProductTypes()
    {
        // Arrange
        var expectedResponse = new List<ProductTypeDto>
    {
        new ProductTypeDto{Id = 1 , Title = "کودک"},
        new ProductTypeDto{Id = 2 , Title = "کوهستان"},
        new ProductTypeDto{Id = 3 , Title = "شهری"}
    };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetAllProductTypeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(CancellationToken.None);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var actualResponse = Assert.IsType<List<ProductTypeDto>>(okResult.Value); 

        Assert.Equal(expectedResponse.Count, actualResponse.Count); 

        for (int i = 0; i < expectedResponse.Count; i++)
        {
            Assert.Equal(expectedResponse[i].Id, actualResponse[i].Id); 
            Assert.Equal(expectedResponse[i].Title, actualResponse[i].Title); 
        }
    }

}

