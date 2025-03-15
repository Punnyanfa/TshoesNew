using FCSP.DTOs.Authentication;
using FCSP.DTOs;
using FCSP.Models.Entities;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using FCSP.Repositories;
using FCSP.DTOs.ShippingInfo;

namespace FCSP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthService(IPasswordHashingService passwordHashingService, ITokenService tokenService, IUserRepository userRepository)
    {
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserLoginRequestAsync(request);

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
        catch (Exception ex)
        {
            return new BaseResponseModel<UserLoginResponse>
            {
                Code = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserRegisterRequestAsync(request);

            await _userRepository.AddAsync(user);

            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 200,
                Message = "User registered successfully",
                Data = new UserRegisterResponse
                {
                    Token = _tokenService.GetToken(user),
                },
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserPasswordResponse>> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserPasswordRequestAsync(request);

            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 200,
                Message = "Password updated successfully",
                Data = new UpdateUserPasswordResponse
                {
                    Success = true,
                },
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserInformationResponse>> UpdateUserInformation(UpdateUserInformationRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserInformationRequestAsync(request);

            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 200,
                Message = "User information updated successfully",
                Data = new UpdateUserInformationResponse
                {
                    Success = true,
                },
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<BaseResponseModel<UserDeleteResponse>> DeleteUser(UserDeleteRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserDeleteRequestAsync(request);

            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UserDeleteResponse>
            {
                Code = 200,
                Message = "User deleted successfully",
                Data = new UserDeleteResponse
                {
                    Success = true,
                },
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserDeleteResponse>
            {
                Code = 500,
                Message = ex.Message,
            };
        }
    }

    private async Task<User> GetUserEntityFromUserLoginRequestAsync(UserLoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        return user;
    }

    private async Task<User> GetUserEntityFromUserRegisterRequestAsync(UserRegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        return new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHashingService.GetHashedPassword(request.Password),
            UserRole = Common.Enums.UserRole.Customer,
        };
    }

    private async Task<User> GetUserEntityFromUpdateUserPasswordRequestAsync(UpdateUserPasswordRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);

        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (string.IsNullOrEmpty(request.NewPassword))
        {
            throw new InvalidOperationException("New password is required to change password");
        }

        if (request.NewPassword.Length < 8)
        {
            throw new InvalidOperationException("New password must be at least 8 characters long");
        }

        if (string.IsNullOrEmpty(request.CurrentPassword))
        {
            throw new InvalidOperationException("Current password is required to change password");
        }

        if (!_passwordHashingService.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        user.PasswordHash = _passwordHashingService.GetHashedPassword(request.NewPassword);
        user.UpdatedAt = DateTime.Now;

        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserInformationRequestAsync(UpdateUserInformationRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);

        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        return new User
        {
            Id = request.Id,
            Name = request.Name ?? user.Name,
            Gender = request.Gender ?? user.Gender,
            Dob = request.Dob ?? user.Dob,
            UpdatedAt = DateTime.Now,
        };
    }

    private async Task<User> GetUserEntityFromUserDeleteRequestAsync(UserDeleteRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);

        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (!_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Password is incorrect");
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.Now;

        return user;
    }
}

