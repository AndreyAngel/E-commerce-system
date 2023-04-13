using AutoMapper;
using IdentityAPI.DataBase;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityAPI.Test.UserServiceTests;

public static class UserServiceBuilder
{
    public static Mock<IConfiguration> MockConfiguration { get => new(); }

    public static Mock<IBusControl> MockBus { get => new(); }

    public static Mock<ICustomUserStore> MockUserStore { get => new(); }

    public static Mock<IOptions<IdentityOptions>> MockOptions { get => new(); }

    public static Mock<IPasswordHasher<User>> MockPasswordHasher { get => new(); }

    public static Mock<IEnumerable<IUserValidator<User>>> MockUserValidator { get => new(); }

    public static Mock<IEnumerable<IPasswordValidator<User>>> MockPasswordValidator { get => new(); }

    public static Mock<ILookupNormalizer> MockNormalized { get => new(); }

    public static Mock<IdentityErrorDescriber> MockErrorDescriber { get => new(); }

    public static Mock<IServiceProvider> MockServiceProvider { get => new(); }

    public static IMapper MockMapper
    {
        get
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            return mappingConfig.CreateMapper();
        }
    }

    public static Mock<ILogger<UserManager<User>>> MockLogger { get => new(); }
}
