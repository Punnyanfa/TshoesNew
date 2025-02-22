using FCSP.DTOs.Authentication;
using FCSP.Models.Entities;
using FCSP.Repositories;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;

namespace FCSP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;

    private readonly IUserRepository _userRepository;

    public AuthService(
        IPasswordHashingService passwordHashingService,
        ITokenService tokenService,
        IUserRepository userRepository)
    {
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request)
    {
        var response = new UserLoginResponse();
        response.Token = _tokenService.GetToken();

        return response;
    }

    public async Task<UserRegisterResponse> Register(UserRegisterRequest request)
    {
        User user = GetUserEntityFromUserRegisterRequest(request);

        await _userRepository.AddAsync(user);

        return new UserRegisterResponse
        {
            Id = user.Id,
            Token = _tokenService.GetToken(),
        };
    }

    private User GetUserEntityFromUserRegisterRequest(UserRegisterRequest request)
    {
        return new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHashingService.GetHashedPassword(request.Password),
        };
    }
}
