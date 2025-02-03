using Application.Features.Basket.Queries.GetBasketById;
using Domain.Entities.BasketEntity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;

namespace Store.tests.Controllers;

public class BasketControllerTest
{
    private readonly Mock<IMediator> _mediator;
    private readonly BasketController _controller;

    public BasketControllerTest()
    {
        _mediator = new Mock<IMediator>();
        _controller = new BasketController();

        //شبیه ساز وابستگی های کنترلر
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IMediator)))
            .Returns(_mediator.Object);
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider.Object };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task Get_Basket_By_Id_Test()
    {
        // Arrange
        string id = "a059a4e5-42a4-4ca4-9b04-204b336c9a6c";
        var response = new CustomerBasket(id)
        {
            Id = id,
            Items = new List<CustomerBasketItem>
        {
            new CustomerBasketItem
            {
               ProductTitle ="دوچرخه دینو مدل 27.5",
               Type = "شهری",
               Brand = "المپیا",
               Quantity = 1,
               Price = 17800000,
               Discount = 0,
               PictureUrl = "http://localhost:9001/images/products/im11.webp",
               Id = 11,
               IsDelete = false
            }
        }
        };

        _mediator.Setup(m => m.Send(It.IsAny<GetBasketByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetBasketById(id, CancellationToken.None); 

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); 
        var returnValue = Assert.IsType<CustomerBasket>(okResult.Value);
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.Items.First().Price, returnValue.Items.First().Price);
        Assert.Equal(response.Items.Count, returnValue.Items.Count);
    }

}
