using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IAuthorsRepository _authorsRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IJwtTokenGenerator jwtTokenGenerator,
        IAuthorsRepository authorsRepository)
        : base(userManager, mapper)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _authorsRepository = authorsRepository;
    }

    public async Task<LoginUserResponse> LoginAsync(UserLoginRequest userLogin)
    {
        var identityUser = await _userManager.FindByEmailAsync(userLogin.Email)
                           ?? throw new NotFoundException(nameof(Author));

        var result = await _userManager.CheckPasswordAsync(identityUser, userLogin.Password);

        if (!result) throw new InvalidEmailOrPasswordException();

        var author = await _authorsRepository.GetAuthorByEmailAsync(userLogin.Email)
                     ?? throw new NotFoundException(nameof(Author));

        var response = _mapper.Map<LoginUserResponse>(author);
        response.Token = _jwtTokenGenerator.GenerateToken(author);

        return response;
    }

    public async Task<NewUserResponse> RegisterAsync(UserRegisterRequest userRegister)
    {
        var identityUser = new IdentityUser
        {
            Email = userRegister.Email,
            UserName = userRegister.FullName
        };

        var result = await _userManager.CreateAsync(identityUser, userRegister.Password);

        if (!result.Succeeded)
        {
            var errorsMessages = result.Errors.Select(e => e.Description);

            throw new BadRequestException(string.Join(' ', errorsMessages));
        }

        var author = _mapper.Map<Author>(userRegister);

        await _authorsRepository.AddAsync(author);

        var response = _mapper.Map<NewUserResponse>(author);
        response.Token = _jwtTokenGenerator.GenerateToken(author);

        return response;
    }
}