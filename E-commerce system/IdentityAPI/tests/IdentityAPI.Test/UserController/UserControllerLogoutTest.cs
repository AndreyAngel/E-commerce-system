using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTest;

public class UserControllerLogoutTest
{
    private static IMapper _mapper;

    public UserControllerLogoutTest()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    public async void Logout_Returns_NoContent()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Logout(Guid.NewGuid()));

        var controller = new UserController(mockService.Object, _mapper);

        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.HttpContext.Items["User"] = new User() { Id = Guid.NewGuid().ToString() };

        // Act
        var result = controller.Logout();

        // Assert
        var actionResult = Assert.IsType<NoContentResult>(result);
    }
}
