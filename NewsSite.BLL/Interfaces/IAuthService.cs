using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces;

public interface IAuthService
{
    Task<LoginUserResponse> LoginAsync(UserLoginRequest userLogin);

    Task<NewUserResponse> RegisterAsync(UserRegisterRequest userRegister);
}