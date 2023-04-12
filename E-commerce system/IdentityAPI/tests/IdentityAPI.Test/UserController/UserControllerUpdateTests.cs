using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTest;

public class UserControllerUpdateTests
{
    private static IMapper _mapper;

    public UserControllerUpdateTests()
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
    public async void Update_Returns_OkObjectResult()
    {
        // Arrange
        var model = CreateUserDTORequest();
        var user = _mapper.Map<User>(model);
        var userId = Guid.NewGuid();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Update(user, userId));
        mockService.Setup(x => x.GetById(userId)).Returns(GetTestUserById());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Update(model, userId);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<UserDTOResponse>(actionResult.Value);
    }

    [Fact]
    public async void Update_Returns_BadRequestResult()
    {
        // Arrange
        var model = CreateUserDTORequest();
        var user = _mapper.Map<User>(model);
        var userId = Guid.NewGuid();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Update(user, userId)).Returns(UpdateWithErrors());

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Update(model, userId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void Update_Returns_NotFoundResult()
    {
        // Arrange
        var model = CreateUserDTORequest();
        var user = _mapper.Map<User>(model);
        var userId = Guid.NewGuid();

        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.Update(user, userId)).Throws<NotFoundException>();

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.Update(model, userId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private static UserDTORequest CreateUserDTORequest()
    {
        return new UserDTORequest()
        {
            Name = "name"
        };
    }

    private static async Task<IIdentityDTOResponse> UpdateWithErrors()
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

    private static async Task<User> GetTestUserById()
    {
        return new User()
        {
            Id = "cec7fee8-f13d-4274-b4e5-fb11074fce9d",
            Name = "Andrey",
            Surname = "Zakharov",
            AddressId = Guid.Parse("6AAAF302-EE45-4C96-B425-1A729EDDBA9B"),
            Address = new Address()
            {
                City = "Tomsk",
                Street = "Litkina",
                NumberOfHome = "8",
                ApartmentNumber = "811"
            },
            BirthDate = new DateTime(2004 - 03 - 31),
            RegistrationDate = new DateTime(2023 - 04 - 06),
            UserName = "admin@admin.ru",
            NormalizedUserName = "ADMIN@ADMIN.RU",
            Email = "admin@admin.ru",
            NormalizedEmail = "ADMIN@ADMIN.RU",
            PasswordHash = "AQAAAAIAAYagAAAAEAzbqUZj3x1wjLyTjnNhRixSExXvFrbfXZpNDBxHHuYWEBsm/JMJeBQieB0Ml5Q+aQ=="
        };
    }
}
