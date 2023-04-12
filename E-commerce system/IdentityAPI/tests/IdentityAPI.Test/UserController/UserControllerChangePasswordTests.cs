using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Models.DTO.Requests;
using IdentityAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTest;

public class UserControllerChangePasswordTests
{
    private static IMapper _mapper;

    public UserControllerChangePasswordTests()
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
    public async void CahngePassword_Returns_NoContentResult()
    {
        // Arrange
        var model = CreateChangePassordDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.ChangePassword(model.Email, model.OldPassword, model.NewPassword));

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.ChangePassword(model);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void ChangePassword_Returns_BadRequestObjectResult()
    {
        // Arrange
        var model = CreateChangePassordDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.ChangePassword(model.Email, model.OldPassword, model.NewPassword))
            .Returns(ChangePasswordErrors());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.ChangePassword(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void ChangePassword_Returns_NotFoundObjectResult()
    {
        // Arrange
        var model = CreateChangePassordDTORequest();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.ChangePassword(model.Email, model.OldPassword, model.NewPassword))
            .Throws<NotFoundException>();

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.ChangePassword(model);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    private static ChangePasswordDTORequest CreateChangePassordDTORequest()
    {
        return new()
        {
            Email = "admin@admin.ru",
            OldPassword = "string",
            NewPassword = "admin",
        };
    }

    private static async Task<IIdentityDTOResponse> ChangePasswordErrors()
    {
        return new IdentityErrorsDTOResponse(errors: new List<IdentityError>()
        {
            new IdentityError()
            {
                Code = "400",
                Description = "description"
            }
        });
    }
}
