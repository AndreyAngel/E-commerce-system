using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.Exceptions;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTest;

public class UserControllerLoginTests
{
    private static IMapper _mapper;

    public UserControllerLoginTests()
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
    public async void Login_Returns_AuthorizationDTOResponse()
    {
        // Arrange
        var model = CreateLoginDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Login(model)).Returns(Login());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Login(model);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthorizationDTOResponse>(actionResult.Value);
    }

    [Fact]
    public async void Login_Returns_A_NotFoundResult()
    {
        /// Arrange
        var model = CreateLoginDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Login(model)).Throws(new NotFoundException());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Login(model);

        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void Login_Returns_A_UnauthorizedResult()
    {
        /// Arrange
        var model = CreateLoginDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Login(model)).Throws(new IncorrectPasswordException());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Login(model);

        //Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    private static LoginDTORequest CreateLoginDTORequest()
    {
        return new LoginDTORequest()
        {
            Email = "admin@admin.ru",
            Password = "password"
        };
    }

    private static async Task<AuthorizationDTOResponse> Login()
    {
        return new AuthorizationDTOResponse(accessToken: "fsadadadsa",
                                            refreshToken: "fsadsdasdwefg",
                                            expiresIn: 900,
                                            userId: Guid.NewGuid(),
                                            tokenType: "Bearer");
    }
}
