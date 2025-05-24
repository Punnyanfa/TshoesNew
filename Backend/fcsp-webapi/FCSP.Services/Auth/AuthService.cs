using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Authentication;
using FCSP.DTOs.UserOtp;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
namespace FCSP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IDesignerRepository _designerRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUserOtpRepository _userOtpRepository;
    private readonly string? _azureConnectionString;
    private readonly string? _azureContainerName;

    public AuthService(
        IPasswordHashingService passwordHashingService, 
        ITokenService tokenService, 
        IUserRepository userRepository, 
        IConfiguration configuration, 
        IDesignerRepository designerRepository, 
        IManufacturerRepository manufacturerRepository,
        IEmailService emailService,
        IUserOtpRepository userOtpRepository)
    {
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _configuration = configuration;
        _designerRepository = designerRepository;
        _manufacturerRepository = manufacturerRepository;
        _emailService = emailService;
        _userOtpRepository = userOtpRepository ?? throw new ArgumentNullException(nameof(userOtpRepository));
        _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
        _azureContainerName = _configuration["AzureStorage:ImagesContainer"];
    }

    #region Public Methods
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
                    Status = user.IsBanned ? "Banned" : "Active",
                    CreatedAt = user.CreatedAt.ToString("dd-MM-yyyy"),
                }).ToList()
            }
        };
    }

    public async Task<BaseResponseModel<GetUserByIdResponse>> GetUserById(GetUserByIdRequest request)
    {
        try
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
                    AvatarImageUrl = user.AvatarImageUrl ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty
                }
            };
        }
        catch(Exception ex)
        {
            return new BaseResponseModel<GetUserByIdResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
       
    }

    public async Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request)
    {
        try
        {         
            var user = await GetUserEntityFromUserLoginRequestAsync(request);
            if (user.IsBanned == true)
            {
                return new BaseResponseModel<UserLoginResponse>
                {
                    Code = 401,
                    Message = "This account has been banned"
                };
            }
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
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 400,
                Message = ex.Message,
                Data = new UserRegisterResponse { Success = false }
            };
        }
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 409, // Conflict for existing email
                Message = ex.Message,
                Data = new UserRegisterResponse { Success = false }
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
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = ex.Message.Contains("Admin") ? 400 : (ex.Message.Contains("not found") ? 404 : 400),
                Message = ex.Message,
                Data = ex.Message.Contains("Admin") ? new UpdateUserStatusResponse { Success = false } : null
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserStatusResponse> { Code = 500, Message = ex.Message };
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
                Message = "Success",
                Data = new UpdateUserPasswordResponse { Success = true }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 400,
                Message = ex.Message,
                Data = new UpdateUserPasswordResponse { Success = false }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateUserPasswordResponse { Success = false }
            };
        }
    }
    public async Task<BaseResponseModel<ForgetUserPasswordResponse>> ForgetUserPassword(ForgetUserPasswordRequest request)
    {
        try
        {
            var user = await GetUserEntityFromForgetUserPasswordRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<ForgetUserPasswordResponse>
            {
                Code = 200,
                Message = "Password updated successfully",
                Data = new ForgetUserPasswordResponse
                {
                    Success = true
                }
            };
        }catch (Exception ex)
        {
            return new BaseResponseModel<ForgetUserPasswordResponse>
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
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = ex.Message.Contains("not found") ? 404 : 400,
                Message = ex.Message,
                Data = ex.Message.Contains("not found") ? null : new UpdateUserAvatarResponse { Success = false }
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
                Message = "Success",
                Data = new UpdateUserInformationResponse { Success = true }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 400,
                Message = ex.Message,
                Data = new UpdateUserInformationResponse { Success = false }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateUserInformationResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserRoleResponse>> UpdateUserRole(UpdateUserRoleRequest request)
    {
        try
        {     
            var user = await GetUserEntityFromUpdateUserRoleRequestAsync(request);
            if (request.Role == UserRole.Designer)
            {
                var designer = await GetDesignerEntityFromUpdateUserRoleRequest(request);
                await _designerRepository.AddAsync(designer);                
                await _userRepository.UpdateAsync(user);
            }

            if (request.Role == UserRole.Manufacturer)
            {
                var manufacturer = await GetManufacturerEntityFromUpdateUserRoleRequest(request);
                await _manufacturerRepository.AddAsync(manufacturer);               
                await _userRepository.UpdateAsync(user);
            }
            
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = 200,
                Message = "User role updated successfully",
                Data = new UpdateUserRoleResponse
                {
                    Success = true
                }
            };
        }
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = ex.Message.Contains("not found") ? 404 : 400,
                Message = ex.Message,
                Data = ex.Message.Contains("not found") ? null : new UpdateUserRoleResponse { Success = false }
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

    public async Task<BaseResponseModel<SendEmailResponse>> SendEmailToUser(SendEmailRequest request)
    {
        try
        {
            var user = await _userRepository.FindAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<SendEmailResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = new SendEmailResponse { Success = false }
                };
            }

            var emailSent = await _emailService.SendEmailAsync(
                user.Email,
                request.Subject,
                request.Body,
                request.IsHtml
            );

            return new BaseResponseModel<SendEmailResponse>
            {
                Code = emailSent ? 200 : 500,
                Message = emailSent ? "Email sent successfully" : "Failed to send email",
                Data = new SendEmailResponse { Success = emailSent }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<SendEmailResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new SendEmailResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<GenerateOtpResponse>> GenerateOtpAsync(GenerateOtpRequest request)
    {
        try
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<GenerateOtpResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = null
                };
            }

            // Invalidate any existing OTPs for the same purpose
            var existingOtp = await _userOtpRepository.GetLatestOtpByUserIdAndPurposeAsync(request.UserId, request.PurposeType);
            if (existingOtp != null)
            {
                existingOtp.IsUsed = true;
                await _userOtpRepository.UpdateAsync(existingOtp);
            }

            // Generate new OTP
            string otpCode = GenerateOtpCode();
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(request.ExpiryTimeInMinutes);

            // Create new OTP record
            var newOtp = new UserOtp
            {
                UserId = request.UserId,
                OtpCode = otpCode,
                ExpiryTime = expiryTime,
                IsUsed = false,
                PurposeType = request.PurposeType
            };

            await _userOtpRepository.AddAsync(newOtp);

            await SendEmailToUser(new SendEmailRequest
            {
                UserId = request.UserId,
                Subject = $"OTP Verification for {request.PurposeType}",
                Body = $"Your OTP code is: {otpCode}",
                IsHtml = false
            });

            return new BaseResponseModel<GenerateOtpResponse>
            {
                Code = 200,
                Message = "OTP generated successfully",
                Data = new GenerateOtpResponse
                {
                    OtpCode = otpCode,
                    ExpiryTime = expiryTime
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GenerateOtpResponse>
            {
                Code = 500,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request)
    {
        try
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<VerifyOtpResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = new VerifyOtpResponse { IsValid = false }
                };
            }

            // Verify OTP
            bool isValid = await _userOtpRepository.VerifyOtpAsync(request.UserId, request.OtpCode, request.PurposeType);

            return new BaseResponseModel<VerifyOtpResponse>
            {
                Code = isValid ? 200 : 400,
                Message = isValid ? "OTP verified successfully" : "Invalid or expired OTP",
                Data = new VerifyOtpResponse { IsValid = isValid }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<VerifyOtpResponse>
            {
                Code = 500,
                Message = $"An error occurred: {ex.Message}",
                Data = new VerifyOtpResponse { IsValid = false }
            };
        }
    }
    #endregion

    #region Private methods
    private bool IsValidEmail(string email)
    {
        try { var addr = new System.Net.Mail.MailAddress(email); return addr.Address == email; }
        catch { return false; }
    }

    private bool IsValidPassword(string password)
    {

        return password.Any(char.IsLetter) && password.Any(char.IsDigit);
    }
    private bool IsValidName(string name)
    {
        return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 10 && phoneNumber.Length <= 12;
    }
    private async Task<User> GetUserEntityFromUserLoginRequestAsync(UserLoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Password))
        {
            throw new InvalidOperationException("Password can not be empty");
        }
        if (request.Password.Length < 8)
        {
            throw new InvalidOperationException("Password can not less than 8 character ");
        }
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        return user;
    }

    private async Task<User> GetUserEntityFromUserRegisterRequestAsync(UserRegisterRequest request)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty");

        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email can not be empty");
        if (!IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email format");

        // Validate password
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty");
        if (request.Password.Length < 8)
            throw new ArgumentException("Password can not less than 8 character");
        if (request.Password.Length > 20)
            throw new ArgumentException("Password can not greater than 20 character");
        if (!IsValidPassword(request.Password))
            throw new ArgumentException("Password does not format");

        // Validate confirm password
        if (request.ConfirmPassword != request.Password)
            throw new ArgumentException("Password not match");
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

    private async Task<Designer> GetDesignerEntityFromUpdateUserRoleRequest(UpdateUserRoleRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        var designer = new Designer
        {
            UserId = user.Id,
            Rating = 0,
            Description = string.Empty,
            Status = DesignerStatus.Active,
            CommissionRate = request.CommissionRate.Value
        };

        return designer;
    }

    private async Task<Manufacturer> GetManufacturerEntityFromUpdateUserRoleRequest(UpdateUserRoleRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }
        

        var manufacturer = new Manufacturer
        {
            UserId = user.Id,
            Description = string.Empty,
            Status = ManufacturerStatus.Active,
            CommissionRate = request.CommissionRate.Value
        };

        return manufacturer;
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
        if (request.Id == 0)
        {
            throw new InvalidOperationException("Id can not be 0");
        }
        var user = await _userRepository.FindAsync(new object[] { request.Id });
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (request.Avatar == null)
        {
            throw new InvalidOperationException("Avatar is required");
        }

        // Validate file type (case-insensitive)
        var contentType = request.Avatar.ContentType?.ToLowerInvariant();
        if (contentType != "image/jpeg" && contentType != "image/png")
        {
            throw new InvalidOperationException("Only JPEG or PNG images are allowed");
        }

        DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
        string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
        string fileName = $"avatar_{user.Id}_{formattedDateTime}.jpeg";
        byte[] fileBytes;

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                Console.WriteLine("Starting CopyToAsync...");
                await request.Avatar.CopyToAsync(memoryStream);
                Console.WriteLine("CopyToAsync completed");
                fileBytes = memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to copy avatar stream: {ex.Message}");
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

        if (user.UserRole == UserRole.Admin)
        {
            throw new InvalidOperationException($"Can't ban Admin");
        }

        if (user.UserRole == UserRole.Manufacturer)
        {
            var manufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(user.Id);
            if (manufacturer != null)
            {
                manufacturer.Status = ManufacturerStatus.Suspended;
                await _manufacturerRepository.UpdateAsync(manufacturer);
            }
        }

        if (user.UserRole == UserRole.Designer)
        {
            var designer = await _designerRepository.GetDesignerByUserIdAsync(user.Id);
            if (designer != null)
            {
                designer.Status = DesignerStatus.Suspended;
                await _designerRepository.UpdateAsync(designer);
            }
        }

        user.IsBanned = request.IsBanned;
        user.UpdatedAt = DateTime.UtcNow.AddHours(7);
        return user;
    }

    private async Task<User> GetUserEntityFromForgetUserPasswordRequestAsync(ForgetUserPasswordRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid user forget password request");
        }

        return user;
    }
    private async Task<User> GetUserEntityFromUpdateUserPasswordRequestAsync(UpdateUserPasswordRequest request)
    {
        // Validate Id
        if (request.Id <= 0)
            throw new ArgumentException("Id required");

        // Validate current password
        if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            throw new ArgumentException("Current password is required");
        if (!IsValidPassword(request.CurrentPassword))
            throw new ArgumentException("Current password not format");
        if (request.CurrentPassword.Length < 8)
            throw new ArgumentException("Current password cannot be less than 8 character");
        if (request.CurrentPassword.Length > 20)
            throw new ArgumentException("Current password cannot be greater than 20 character");

        // Validate new password
        if (string.IsNullOrWhiteSpace(request.NewPassword))
            throw new ArgumentException("New password cannot be empty");
        if (!IsValidPassword(request.NewPassword))
            throw new ArgumentException("New password does not format");
        if (request.NewPassword.Length < 8)
            throw new ArgumentException("New password less than 8 character");
        if (request.NewPassword.Length > 20)
            throw new ArgumentException("New password greater than 20 character");


        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
            throw new ArgumentException("Id not found");
        if (!_passwordHashingService.VerifyHashedPassword(request.CurrentPassword, user.PasswordHash))
            throw new ArgumentException("Current password does not match");
        user.PasswordHash = _passwordHashingService.GetHashedPassword(request.NewPassword);
        return user;
    }
    private async Task<User> GetUserEntityFromUpdateUserInformationRequestAsync(UpdateUserInformationRequest request)
    {
        // Validate Id
        if (request.Id <= 0)
            throw new ArgumentException("Id is required");

        // Validate Name
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is not in correct format (Name can not be empty)");
        if (!IsValidName(request.Name))
            throw new ArgumentException("Name is not in correct format (Name can not be empty)");
        if (request.Name.Length < 5)
            throw new ArgumentException("Name can not less than 5 characters");
        if (request.Name.Length > 25)
            throw new ArgumentException("Name can not greater than 25 characters");

        // Validate PhoneNumber
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new ArgumentException("PhoneNumber can not be empty");
        if (!IsValidPhoneNumber(request.PhoneNumber))
            throw new ArgumentException("Invalid phone format");

        // Validate Gender
        if (string.IsNullOrWhiteSpace(request.Gender))
            throw new ArgumentException("Gender can not be empty");
        if (!new[] { "Male", "Female", "Other" }.Contains(request.Gender))
            throw new ArgumentException("Invalid gender value");

        // Validate Dob
        if (string.IsNullOrWhiteSpace(request.Dob))
            throw new ArgumentException("Date of birth can not be empty");
        if (!DateTime.TryParse(request.Dob, out _))
            throw new ArgumentException("Invalid date of birth");

        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
            throw new ArgumentException("Id not found");

        user.Name = request.Name;
        user.PhoneNumber = request.PhoneNumber;
        user.Gender = request.Gender;
        user.Dob = request.Dob;
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
        if (request.Id <= 0 || request.Id == null)
        {
            throw new InvalidOperationException("User Id is required");
        }

        var user = await _userRepository.FindAsync(new object[] { request.Id });
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }
        // Validate role
        if (request.Role != UserRole.Manufacturer && request.Role != UserRole.Designer)
        {
            throw new InvalidOperationException("Role is invalid");
        }
        // Validate CommissionRate
        if (!request.CommissionRate.HasValue)
        {
            throw new InvalidOperationException("Commission is reqiure");
        }
        if (request.CommissionRate.Value < 5)
        {
            throw new InvalidOperationException("commissionRate can not less than 5");
        }
        if (request.CommissionRate.Value > 50)
        {
            throw new InvalidOperationException("commissionRate can not greater than 50");
        }
        user.UserRole = request.Role; 
        user.UpdatedAt = DateTime.Now;
        return user;
    }

    private string GenerateOtpCode()
    {
        // Generate a 6-digit OTP
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return (value % 1000000).ToString("D6"); // Ensure it's a 6-digit number
        }
    }
    #endregion
}