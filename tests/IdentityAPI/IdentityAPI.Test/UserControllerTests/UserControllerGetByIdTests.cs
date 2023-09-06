using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO.Response;
using IdentityAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityAPI.Test.UserControllerTest;

public class UserControllerGetByIdTests
{
    private static IMapper _mapper;

    public UserControllerGetByIdTests()
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
    public async void GetById_Returns_A_UserDTOResponse()
    {
        // Arrange
        string testUserId = "cec7fee8-f13d-4274-b4e5-fb11074fce9d";
        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.GetById(Guid.Parse(testUserId)))
        .Returns(GetTestUserById(testUserId));

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.GetById(Guid.Parse(testUserId));

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<UserDTOResponse>(actionResult.Value);
        Assert.Equal(testUserId, model.Id);
        Assert.Equal("Andrey", model.Name);
    }

    [Fact]
    public async void GetById_Returns_A_NotFoundResult()
    {
        // Arrange
        string testUserId = "30bb4782-da83-492e-b1bf-c6a53ce1e5c2";
        var mockService = new Mock<IUserService>();
        mockService.Setup(x => x.GetById(Guid.Parse(testUserId))).Throws<NotFoundException>();

        var controller = new UserController(mockService.Object, _mapper);

        // Act
        var result = await controller.GetById(Guid.Parse(testUserId));

        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    private static async Task<User> GetTestUserById(string id)
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
