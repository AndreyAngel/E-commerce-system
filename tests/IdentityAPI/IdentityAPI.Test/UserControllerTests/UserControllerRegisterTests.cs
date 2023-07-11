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
        var model = CreateRegisterDTORequest();
        var user = _mapper.Map<User>(model);

        var mockService = new Mock<IUserService>();
        mockService.Setup( x => x.Register(user, model.Password, model.Role))
           .Returns(Register());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Register(model);

        // Assert
        var actionResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<IDTOResponse>(actionResult.Value);
        Assert.Same(Register(), response);
    }

    private static RegisterDTORequest CreateRegisterDTORequest()
    {
        var request = new RegisterDTORequest()
        {
            Email = "admin@admin.ru",
            BirthDate = DateTime.Now,
            Password = "string",
            PasswordConfirm = "string",
            Role = 0
        };

        return request;
    }

    private static async Task<IDTOResponse> Register()
    {
        return new AuthorizationDTOResponse(accessToken: "fsadadadsa",
                                            refreshToken: "fsadsdasdwefg",
                                            expiresIn: 900,
                                            userId: Guid.NewGuid(),
                                            tokenType: "Bearer");
    }
}
