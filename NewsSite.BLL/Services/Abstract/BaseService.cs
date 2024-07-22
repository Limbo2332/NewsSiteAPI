using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace NewsSite.BLL.Services.Abstract;

public abstract class BaseService
{
    protected readonly IMapper _mapper;
    protected readonly UserManager<IdentityUser> _userManager;

    protected BaseService(
        UserManager<IdentityUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
}