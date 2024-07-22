using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.MappingProfiles;
using NewsSite.BLL.MappingProfiles.Resolvers;

namespace NewsSite.UnitTests.Systems.Services.Abstract;

public abstract class BaseServiceTests
{
    protected readonly IMapper _mapper;
    protected readonly Mock<UserManager<IdentityUser>> _userManagerMock;

    protected BaseServiceTests()
    {
        var store = new Mock<IUserStore<IdentityUser>>();

        _userManagerMock =
            new Mock<UserManager<IdentityUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _mapper = SetUpMapper();
    }

    private IMapper SetUpMapper()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.ConstructServicesUsing(_ => new IdentityUserResolver(_userManagerMock.Object));

            cfg.AddProfile<AuthorsProfile>();
            cfg.AddProfile<NewsProfile>();
            cfg.AddProfile<RubricsProfile>();
            cfg.AddProfile<TagsProfile>();
        });

        return new Mapper(mapperConfiguration);
    }
}