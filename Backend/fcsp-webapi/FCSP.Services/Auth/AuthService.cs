using FCSP.DTOs.Authentication;
using FCSP.DTOs;
using FCSP.Models.Entities;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using FCSP.Repositories;

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

    public async Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request)
    {
        
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (string.IsNullOrEmpty(request.Password) || request.Password.Length < 8)
        {
            throw new UnauthorizedAccessException("Password must be at least 8 characters long");
        }
        var token = _tokenService.GetToken(user);

        return new BaseResponseModel<UserLoginResponse>
        {
            Code = 200,
            Message = "Login successful",
            Data = new UserLoginResponse
            {
                Token = token,
            },
        };
    }

    public async Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request)
    {
        User user = GetUserEntityFromUserRegisterRequest(request);

        await _userRepository.AddAsync(user);

        return new BaseResponseModel<UserRegisterResponse>
        {
            Data = new UserRegisterResponse
            {
                Token = _tokenService.GetToken(user),
            },
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

