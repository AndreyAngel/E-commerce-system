using AutoMapper;
using IdentityAPI.Controllers;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IdentityAPI.Test.UserServiceTests;

public class UserServiceGetByIdTests
{
    /*[Fact]
    public async void Update_Returns_OkObjectResult()
    {
        // Arrange

        var service = new UserService(configuration: UserServiceBuilder.MockConfiguration,
                                      bus: UserServiceBuilder.MockBus,
                                      store: UserServiceBuilder.MockUserStore,
                                      optionsAccessor: UserServiceBuilder.MockOptions,
                                      passwordHasher: UserServiceBuilder.MockPasswordHasher,
                                      userValidators: UserServiceBuilder.MockUserValidator,
                                      passwordValidators: UserServiceBuilder.MockPasswordValidator,
                                      keyNormalizer: UserServiceBuilder.MockNormalized,
                                      errors: UserServiceBuilder.MockErrorDescriber,
                                      services: UserServiceBuilder.MockServiceProvider,
                                      mapper: UserServiceBuilder.MockMapper,
                                      logger: UserServiceBuilder.MockLogger);

        // Act
        var result = await controller.Update(model, userId);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<UserDTOResponse>(actionResult.Value);
    }*/
}
