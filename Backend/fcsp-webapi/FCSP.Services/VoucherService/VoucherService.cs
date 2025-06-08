using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCSP.Common.Utils;

namespace FCSP.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IConfiguration _configuration;
        private const int MAX_EXPIRY_DAYS = 30;

        public VoucherService(IVoucherRepository voucherRepository, IConfiguration configuration)
        {
            _voucherRepository = voucherRepository;
        }

        #region Public Methods

        public async Task<BaseResponseModel<List<GetAllVoucherResponse>>> GetAllVouchers()
        {
            try
            {
                var voucherResponses = await GetAllVouchersAsync();
                return new BaseResponseModel<List<GetAllVoucherResponse>>
                {
                    Code = 200,
                    Message = "Vouchers retrieved successfully",
                    Data = voucherResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetAllVoucherResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetVoucherByCodeResponse>> GetVoucherByCode(GetVoucherByCodeRequest request)
        {
            try
            {
                var voucher = await _voucherRepository.GetAll()
                                                      .Include(v => v.Orders)
                                                      .FirstOrDefaultAsync(o => o.VoucherName == request.Code);
                if (voucher == null)
                {
                    return new BaseResponseModel<GetVoucherByCodeResponse>
                    {
                        Code = 404,
                        Message = "Voucher not found",
                        Data = null
                    };
                }
                
                if(voucher.Status == (int)VoucherStatus.Expired)
                {
                    return new BaseResponseModel<GetVoucherByCodeResponse>
                    {
                        Code = 200,
                        Message = "Voucher is expired",
                        Data = new GetVoucherByCodeResponse
                        {
                            Id = voucher.Id,
                            IsValid = false
                        }
                    };
                }

                if (voucher.Status == (int)VoucherStatus.Used)
                {
                    return new BaseResponseModel<GetVoucherByCodeResponse>
                    {
                        Code = 200,
                        Message = "Voucher is used",
                        Data = new GetVoucherByCodeResponse
                        {
                            Id = voucher.Id,
                            IsValid = false
                        }
                    };
                }

                return new BaseResponseModel<GetVoucherByCodeResponse>
                {
                    Code = 200,
                    Message = "Voucher retrieved successfully",
                    Data = new GetVoucherByCodeResponse
                    {
                        Id = voucher.Id,
                        IsValid = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetVoucherByCodeResponse>
                {
                    Code = 500,
                    Message = "Error retrieving voucher by code. Error: " + ex.Message
                };
            }
        }

        public async Task<BaseResponseModel<GetVoucherByIdResponse>> GetVoucherById(GetVoucherByIdRequest request)
        {
            try
            {
                var voucher = await GetVoucherEntityById(request);
                var response = MapToVoucherByIdResponse(voucher);
                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 200,
                    Message = "Voucher retrieved successfully",
                    Data = response
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetVoucherByOrderIdResponse>> GetVoucherByOrderId(GetVoucherByOrderIdRequest request)
        {
            try
            {
                var voucher = await GetVoucherByOrderIdAsync(request);
                var response = MapToVoucherByOrderIdResponse(voucher);
                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 200,
                    Message = "Voucher retrieved successfully",
                    Data = response
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddVoucherResponse>> AddVoucher(AddVoucherRequest request)
        {
            try
            {
                var voucher = await CreateVoucherFromRequest(request);
                var addedVoucher = await _voucherRepository.AddAsync(voucher);
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 200,
                    Message = "Voucher created successfully",
                    Data = new AddVoucherResponse { VoucherId = addedVoucher.Id }
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = ex.Message.Contains("not found")? 404 : 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateVoucherResponse>> UpdateVoucher(UpdateVoucherRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    throw new ArgumentException("Id can not be zero");
                }
                var voucher = await _voucherRepository.FindAsync(request.Id);
                if (voucher == null) {
                    throw new InvalidOperationException("Voucher not found");
                }
                UpdateVoucherFromRequest(voucher, request);
                await _voucherRepository.UpdateAsync(voucher);
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 200,
                    Message = "Voucher updated successfully",
                    Data = new UpdateVoucherResponse { VoucherId = voucher.Id }
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteVoucherResponse>> DeleteVoucher(DeleteVoucherRequest request)
        {
            try
            {
                var voucher = await GetDeleteVoucherRequestAsync(new DeleteVoucherRequest { Id = request.Id });
                await _voucherRepository.DeleteAsync(voucher.Id);
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 200,
                    Message = "Voucher deleted successfully",
                    Data = new DeleteVoucherResponse { Success = true }
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<int>> UpdateExpiredVouchers()
        {
            try
            {
                var count = await _voucherRepository.UpdateExpiredVouchersAsync();
                return new BaseResponseModel<int>
                {
                    Code = 200,
                    Message = $"Successfully updated {count} expired vouchers",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<int>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = 0
                };
            }
        }

        public async Task<BaseResponseModel<List<GetVoucherByIdResponse>>> GetNonExpiredVouchers()
        {
            try
            {
                var voucherResponses = await GetNonExpiredVouchersAsync();
                return new BaseResponseModel<List<GetVoucherByIdResponse>>
                {
                    Code = 200,
                    Message = "Non-expired vouchers retrieved successfully",
                    Data = voucherResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetVoucherByIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<GetAllVoucherResponse>> GetAllVouchersAsync()
        {
            var vouchers = await _voucherRepository.GetAllVoucherAsync();
            return vouchers.Select(voucher => new GetAllVoucherResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                Status = voucher.Status.ToString(),
                ExpiryDate = voucher.ExpirationDate,
                IsUsed = voucher.Orders != null && voucher.Orders.Any()
            }).ToList();
        }

        private async Task<DeleteVoucherRequest> GetDeleteVoucherRequestAsync(DeleteVoucherRequest request)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var voucher = await _voucherRepository.FindAsync(request.Id);
            if (voucher == null)
            {
                throw new InvalidOperationException("Voucher not found");
            }
            return request;
        }
        private async Task<Voucher> GetVoucherEntityById(GetVoucherByIdRequest request)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Voucher ID must be greater than 0", nameof(request.Id));
            }

            var voucher = await _voucherRepository.GetAll()
                                                  .Include(v => v.Orders)
                                                  .FirstOrDefaultAsync(o => o.Id == request.Id);
            if (voucher == null)
            {
                throw new InvalidOperationException($"Voucher with ID {request.Id} not found");
            }

            return voucher;
        }

        private async Task<Voucher> GetVoucherByOrderIdAsync(GetVoucherByOrderIdRequest request)
        {
            if (request.OrderId <= 0)
            {
                throw new ArgumentException("Order ID must be greater than 0", nameof(request.OrderId));
            }

            var voucher = await _voucherRepository.GetVoucherByOrderIdAsync(request.OrderId);
            if (voucher == null)
            {
                throw new InvalidOperationException($"No voucher found for order ID {request.OrderId}");
            }

            return voucher;
        }

        private async Task<List<GetVoucherByIdResponse>> GetNonExpiredVouchersAsync()
        {
            var vouchers = await _voucherRepository.GetNonExpiredVouchersAsync();
            return vouchers.Select(voucher => new GetVoucherByIdResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                Status = voucher.Status.ToString(),
                ExpiryDate = voucher.ExpirationDate,
                IsUsed = voucher.Orders != null && voucher.Orders.Any(),
                OrderIds = voucher.Orders?.Select(o => o.Id).ToList() ?? new List<long>()
            }).ToList();
        }

        private async Task<Voucher> CreateVoucherFromRequest(AddVoucherRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                throw new InvalidOperationException("Code cannot be null or empty");
            }
            if (request.Code.Length < 5)
            {
                throw new InvalidOperationException("Code must be at least 5 characters");
            }
            if (request.Code.Length > 20)
            {
                throw new InvalidOperationException("Code must be greater than 20 characters");
            }
            if (request.Code.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)))           
            {
                throw new InvalidOperationException("Code should not include special character");
            }
            if (request.DiscountAmount <= 0 || request.DiscountAmount == null)
            {
                throw new InvalidOperationException("Discount amount must be greater than 0");
            }
            if (request.DiscountAmount < 0)
            {
                throw new InvalidOperationException("Discount amount must be greater than 0");
            }
            if(request.DiscountAmount > 30)
            {
                throw new InvalidOperationException("Discount amount must be less than or equal to 30");
            }
            if (request.ExpiryDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("expiryDate can not be the past ");
            }
            var existingVoucher = await _voucherRepository.FindAsync(request.Code);
            if (existingVoucher != null)
            {
                throw new InvalidOperationException($"Voucher with code {request.Code} already exists");
            }
            ValidateExpirationDate(request.ExpiryDate);
            return new Voucher
            {
                VoucherName = request.Code,
                VoucherValue = request.DiscountAmount.ToString(),
                ExpirationDate = request.ExpiryDate,
                Status = (int)VoucherStatus.Active,
                Description = request.Code,
                CreatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7()
            };
        }

        private void UpdateVoucherFromRequest(Voucher voucher, UpdateVoucherRequest request)
        {
                
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                throw new ArgumentException("Code cannot be null or empty");
            }
            if (request.Code.Length < 5)
            {
                throw new ArgumentException("Code must be at least 5 characters");
            }
            if (request.Code.Length > 20)
            {
                throw new ArgumentException("Code must be less than or equal to 20 characters");
            }
            if (request.Code.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)))
            {
                throw new ArgumentException("Code should not include special character");
            }
            if (request.DiscountAmount <= 0 || request.DiscountAmount == null)
            {
                throw new ArgumentException("Discount amount must be greater than 0");
            }           
            if (request.DiscountAmount > 30)
            {
                throw new ArgumentException("Discount amount must be less than or equal to 30");
            }
            if(request.ExpiryDate == null)
            {
                throw new ArgumentException("Expiry date cannot be null");
            }
            if (request.ExpiryDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Expiry date cannot be in the past");
            }
            ValidateExpirationDate(request.ExpiryDate, voucher.CreatedAt);
            voucher.VoucherName = request.Code;
            voucher.VoucherValue = request.DiscountAmount.ToString();
            voucher.ExpirationDate = request.ExpiryDate;
            voucher.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        }

        private GetVoucherByIdResponse MapToVoucherByIdResponse(Voucher voucher)
        {
            return new GetVoucherByIdResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                Status = voucher.Status.ToString(),
                ExpiryDate = voucher.ExpirationDate,
                IsUsed = voucher.Orders != null && voucher.Orders.Any(),
                OrderIds = voucher.Orders?.Select(o => o.Id).ToList() ?? new List<long>()
            };
        }

        private GetVoucherByOrderIdResponse MapToVoucherByOrderIdResponse(Voucher voucher)
        {
            return new GetVoucherByOrderIdResponse
            {
                Id = voucher.Id,
                Code = voucher.VoucherName ?? string.Empty,
                DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                ExpiryDate = voucher.ExpirationDate,
                Status = voucher.Status.ToString()
            };
        }

        private void ValidateExpirationDate(DateTime expiryDate, DateTime? createdAt = null)
        {
            var now = DateTimeUtils.GetCurrentGmtPlus7();
            createdAt ??= now;

            if (expiryDate < now)
            {
                throw new ArgumentException("Expiration date cannot be in the past", nameof(expiryDate));
            }

            var maxExpiryDate = createdAt.Value.AddDays(MAX_EXPIRY_DAYS);
            if (expiryDate > maxExpiryDate)
            {
                throw new ArgumentException($"Expiration date cannot be more than {MAX_EXPIRY_DAYS} days from creation date", nameof(expiryDate));
            }
        }

        #endregion
    }
}