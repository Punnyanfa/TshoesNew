using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FCSP.Common.Utils;

namespace FCSP.Repositories.Implementations
{
    public class UserOtpRepository : GenericRepository<UserOtp>, IUserOtpRepository
    {
        public UserOtpRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<UserOtp> GetLatestOtpByUserIdAndPurposeAsync(long userId, string purpose)
        {
            return await Entities
                .Where(o => o.UserId == userId && o.PurposeType == purpose && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<UserOtp> GetByOtpCodeAsync(string otpCode)
        {
            return await Entities
                .FirstOrDefaultAsync(o => o.OtpCode == otpCode && !o.IsUsed);
        }

        public async Task<bool> VerifyOtpAsync(long userId, string otpCode, string purpose)
        {
            var otp = await Entities
                .FirstOrDefaultAsync(o => 
                    o.UserId == userId && 
                    o.OtpCode == otpCode && 
                    o.PurposeType == purpose && 
                    !o.IsUsed && 
                    o.ExpiryTime > DateTimeUtils.GetCurrentGmtPlus7());

            if (otp == null)
            {
                return false;
            }

            otp.IsUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 