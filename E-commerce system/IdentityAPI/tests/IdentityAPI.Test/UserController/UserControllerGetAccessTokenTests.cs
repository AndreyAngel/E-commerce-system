using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security;

namespace IdentityAPI.Test.UserControllerTest;
public class UserControllerGetAccessTokenTests
{
    private static IMapper _mapper;

    public UserControllerGetAccessTokenTests()
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
    public async void GetAccessToken_Returns_AuthorizationDTOResponse()
    {
        // Arrange
        var model = CreateGetAccessTokenDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.GetAccessToken(model.RefreshToken)).Returns(Login());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.GetAccessToken(model);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthorizationDTOResponse>(actionResult.Value);
    }

    [Fact]
    public async void Login_Returns_A_ForbidResult()
    {
        /// Arrange
        var model = CreateGetAccessTokenDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.GetAccessToken(model.RefreshToken)).Throws(new SecurityException());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.GetAccessToken(model);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void Login_Returns_A_UnauthorizedResult()
    {
        /// Arrange
        var model = CreateGetAccessTokenDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.GetAccessToken(model.RefreshToken)).Throws(new UnauthorizedAccessException());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.GetAccessToken(model);

        //Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    private static GetAccessTokenDTORequest CreateGetAccessTokenDTORequest()
    {
        return new GetAccessTokenDTORequest()
        {
            RefreshToken = "fsadsdasdwefg"
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
