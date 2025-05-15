using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IUserOtpRepository : IGenericRepository<UserOtp>
    {
        Task<UserOtp> GetLatestOtpByUserIdAndPurposeAsync(long userId, string purpose);
        Task<UserOtp> GetByOtpCodeAsync(string otpCode);
        Task<bool> VerifyOtpAsync(long userId, string otpCode, string purpose);
    }
} 