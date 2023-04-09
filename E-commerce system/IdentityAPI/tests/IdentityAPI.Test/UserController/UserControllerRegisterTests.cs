using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTests;

public class UserControllerRegisterTests
{
    private static IMapper _mapper;

    public UserControllerRegisterTests()
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
    public async void Register_Returns_AuthorizationDTOResponse()
    {
        // Arrange
        var request = CreateRegisterDTORequest();
        var user = _mapper.Map<User>(request);

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Register(user, request.Password, request.Role))
           .ReturnsAsync(Register);

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Register(request);

        // Assert
        var actionResult = Assert.IsType<CreatedResult>(result);
        var model = Assert.IsType<AuthorizationDTOResponse>(actionResult.Value);
        Assert.Same(Register(), model);
    }

    private RegisterDTORequest CreateRegisterDTORequest()
    {
        return new RegisterDTORequest()
        {
            Email = "admin@admin.ru",
            BirthDate = DateTime.Now,
            Password = "password",
            PasswordConfirm = "password",
            Role = 0
        };
    }

    private AuthorizationDTOResponse Register()
    {
        return new AuthorizationDTOResponse(accessToken: "fsadadadsa",
                                            refreshToken: "fsadsdasdwefg",
                                            expiresIn: 900,
                                            userId: Guid.NewGuid(),
                                            tokenType: "Bearer");
    }
}
