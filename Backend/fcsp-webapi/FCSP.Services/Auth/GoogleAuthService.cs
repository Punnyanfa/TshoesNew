using FCSP.DTOs.Authentication;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using FCSP.Common.Utils;

namespace FCSP.Services.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public GoogleAuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserLoginResponse> GoogleLogin(string token)
    {
        try
        {
            // Validate Google ID token
            var payload = await ValidateGoogleToken(token);

            // Check if user exists by email
            var user = await _userRepository.GetByEmailAsync(payload.Email);

            if (user == null)
            {
                // Create new user
                user = new User
                {
                    Email = payload.Email,
                    Name = payload.Name ?? payload.Email.Split('@')[0],
                    UserRole = Common.Enums.UserRole.Customer, // Default role
                    IsBanned = false,
                    IsDeleted = false,
                    CreatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                    UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                    AvatarImageUrl = payload.Picture 
                };

                await _userRepository.AddAsync(user);
            }
            else if (user.IsBanned || user.IsDeleted)
            {
                throw new UnauthorizedAccessException("User account is banned or deleted.");
            }

            // Generate JWT token
            var jwtToken = GenerateJwtToken(user);

            return new UserLoginResponse
            {
                Token = jwtToken
            };
        }
        catch (InvalidJwtException)
        {
            throw new UnauthorizedAccessException("Invalid Google token.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Google login failed: {ex.Message}");
        }
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _configuration["Authentication:Google:ClientId"] }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        return payload;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTimeUtils.GetCurrentGmtPlus7().AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}