using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Authentication;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using Microsoft.Extensions.Configuration;
namespace FCSP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly string? _azureConnectionString;
    private readonly string? _azureContainerName;

    public AuthService(IPasswordHashingService passwordHashingService, ITokenService tokenService, IUserRepository userRepository, IConfiguration configuration)
    {
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _configuration = configuration;
        _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
        _azureContainerName = _configuration["AzureStorage:ContainerName"];
    }

    public string HashPassword(string password)
    {
        return _passwordHashingService.GetHashedPassword(password);
    }

    public async Task<BaseResponseModel<GetAllUsersResponse>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return new BaseResponseModel<GetAllUsersResponse>
        {
            Code = 200,
            Message = "Get all users successfully",
            Data = new GetAllUsersResponse
            {
                Users = users.Select(user => new UsersInfo
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.UserRole.ToString(),
                    Status = user.IsDeleted ? "Inactive" : "Active",
                    CreatedAt = user.CreatedAt.ToString("dd-MM-yyyy"),
                }).ToList()
            }
        };
    }

    public async Task<BaseResponseModel<GetUserByIdResponse>> GetUserById(GetUserByIdRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        return new BaseResponseModel<GetUserByIdResponse>
        {
            Code = 200,
            Message = "Get user by id successfully",
            Data = new GetUserByIdResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Dob = user.Dob ?? string.Empty,
                Gender = user.Gender ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            }
        };
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
                Data = new UserLoginResponse { Token = token }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserLoginResponse>
            {
                Code = 500,
                Message = ex.Message
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
                Data = new UserRegisterResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UserRegisterResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserStatusResponse>> UpdateUserStatus(UpdateUserStatusRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserStatusRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = 200,
                Message = "User status updated successfully",
                Data = new UpdateUserStatusResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = 500,
                Message = ex.Message
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
                Data = new UpdateUserPasswordResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserBalanceResponse>> UpdateUserBalance(UpdateUserBalanceRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserBalanceRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserBalanceResponse>
            {
                Code = 200,
                Message = "User balance updated successfully",
                Data = new UpdateUserBalanceResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserBalanceResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserAvatarResponse>> UpdateUserAvatar(UpdateUserAvatarRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserAvatarRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = 200,
                Message = "User avatar updated successfully",
                Data = new UpdateUserAvatarResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = 500,
                Message = ex.Message
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
                Data = new UpdateUserInformationResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 500,
                Message = ex.Message
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
                Data = new UserDeleteResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserDeleteResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserRoleResponse>> UpdateUserRole(UpdateUserRoleRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserRoleRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = 200,
                Message = "User role updated successfully",
                Data = new UpdateUserRoleResponse
                {
                    Success = true,
                    NewRole = user.UserRole
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateUserRoleResponse { Success = false }
            };
        }
    }

    private async Task<User> GetUserEntityFromUserLoginRequestAsync(UserLoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
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
            Balance = 0,
            UserRole = UserRole.Customer,
            IsBanned = false,
            IsDeleted = false
        };
    }

    private async Task<User> GetUserEntityFromUpdateUserBalanceRequestAsync(UpdateUserBalanceRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        user.Balance = request.Balance;
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserAvatarRequestAsync(UpdateUserAvatarRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (request.Avatar == null)
        {
            throw new InvalidOperationException("Avatar is required");
        }

        DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
        string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
        string fileName = $"avatar_{user.Id}_{formattedDateTime}.jpeg";
        byte[] fileBytes;

        using (var memoryStream = new MemoryStream())
        {
            await request.Avatar.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var avatarPath = await UploadToAzureStorage(fileName, fileBytes);

        user.AvatarImageUrl = avatarPath;
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private async Task<string> UploadToAzureStorage(string fileName, byte[] fileBytes)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = "image/jpeg"
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
    }

    private async Task<User> GetUserEntityFromUpdateUserStatusRequestAsync(UpdateUserStatusRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        user.IsBanned = request.IsBanned;
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserPasswordRequestAsync(UpdateUserPasswordRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }
        if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length < 8 || string.IsNullOrEmpty(request.CurrentPassword) || !_passwordHashingService.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid password update request");
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

        user.Name = request.Name ?? user.Name;
        user.Gender = request.Gender ?? user.Gender;
        user.Dob = request.Dob ?? user.Dob;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private async Task<User> GetUserEntityFromUserDeleteRequestAsync(UserDeleteRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid user deletion request");
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserRoleRequestAsync(UpdateUserRoleRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        user.UserRole = request.Role; // C?p nh?t UserRole
        user.UpdatedAt = DateTime.Now;
        return user;
    }
}