using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FCSP.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private const int MAX_EXPIRY_DAYS = 30;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<BaseResponseModel<List<GetAllVoucherResponse>>> GetAllVouchers()
        {
            try
            {
                var vouchers = await _voucherRepository.GetAllVoucherAsync();
                var voucherResponses = vouchers.Select(voucher => new GetAllVoucherResponse(

                    )
                {
                    Id = voucher.Id,
                    Code = voucher.VoucherName ?? string.Empty,
                    DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                    Status = voucher.Status.ToString(),
                    ExpiryDate = voucher.ExpirationDate,
                    IsUsed = voucher.Orders != null && voucher.Orders.Any()
                }).ToList();

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
                    Message = $"Error retrieving vouchers: {ex.Message}"
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
                var voucher = await _voucherRepository.GetAll()
                                                      .Include(v => v.Orders)
                                                      .FirstOrDefaultAsync(o => o.Id == request.Id);
                if (voucher == null)
                {
                    return new BaseResponseModel<GetVoucherByIdResponse>
                    {
                        Code = 404,
                        Message = "Voucher not found"
                    };
                }

                var response = new GetVoucherByIdResponse()
                {
                    Id = voucher.Id,
                    Code = voucher.VoucherName ?? string.Empty,
                    DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                    Status = voucher.Status.ToString(),
                    ExpiryDate = voucher.ExpirationDate,
                    IsUsed = voucher.Orders != null && voucher.Orders.Any(),
                    OrderIds = voucher.Orders?.Select(o => o.Id).ToList() ?? new List<long>()
                };

                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 200,
                    Message = "Voucher retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetVoucherByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving voucher: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<GetVoucherByOrderIdResponse>> GetVoucherByOrderId(GetVoucherByOrderIdRequest request)
        {
            try
            {
                var voucher = await _voucherRepository.GetVoucherByOrderIdAsync(request.OrderId);
                if (voucher == null)
                {
                    return new BaseResponseModel<GetVoucherByOrderIdResponse>
                    {
                        Code = 404,
                        Message = "No voucher found for this order"
                    };
                }

                var response = new GetVoucherByOrderIdResponse()
                {
                    Id = voucher.Id,
                    Code = voucher.VoucherName ?? string.Empty,
                    DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                    ExpiryDate = voucher.ExpirationDate,
                    Status = voucher.Status.ToString()
                };

                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 200,
                    Message = "Voucher retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetVoucherByOrderIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving voucher: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddVoucherResponse>> AddVoucher(AddVoucherRequest request)
        {
            try
            {
                ValidateExpirationDate(request.ExpiryDate);

                var voucher = new Voucher
                {
                    VoucherName = request.Code,
                    VoucherValue = request.DiscountAmount.ToString(),
                    ExpirationDate = request.ExpiryDate,
                    Status = (int)VoucherStatus.Active,
                    Description = request.Code
                };

                var addedVoucher = await _voucherRepository.AddAsync(voucher);
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 201,
                    Message = "Voucher created successfully",
                    Data = new AddVoucherResponse { VoucherId = addedVoucher.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 500,
                    Message = $"Error creating voucher: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<UpdateVoucherResponse>> UpdateVoucher(UpdateVoucherRequest request)
        {
            try
            {
                var voucher = await _voucherRepository.FindAsync(request.Id);
                if (voucher == null)
                {
                    return new BaseResponseModel<UpdateVoucherResponse>
                    {
                        Code = 404,
                        Message = "Voucher not found"
                    };
                }

                try
                {
                    ValidateExpirationDate(request.ExpiryDate, voucher.CreatedAt);
                }
                catch (ArgumentException ex)
                {
                    return new BaseResponseModel<UpdateVoucherResponse>
                    {
                        Code = 400,
                        Message = ex.Message
                    };
                }

                voucher.VoucherName = request.Code;
                voucher.VoucherValue = request.DiscountAmount.ToString();
                voucher.ExpirationDate = request.ExpiryDate;
                voucher.UpdatedAt = DateTime.Now;

                await _voucherRepository.UpdateAsync(voucher);
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 200,
                    Message = "Voucher updated successfully",
                    Data = new UpdateVoucherResponse { VoucherId = voucher.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateVoucherResponse>
                {
                    Code = 500,
                    Message = $"Error updating voucher: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<DeleteVoucherResponse>> DeleteVoucher(DeleteVoucherRequest request)
        {
            try
            {
                var voucher = await _voucherRepository.FindAsync(request.Id);
                if (voucher == null)
                {
                    return new BaseResponseModel<DeleteVoucherResponse>
                    {
                        Code = 404,
                        Message = "Voucher not found"
                    };
                }

                await _voucherRepository.DeleteAsync(request.Id);
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 200,
                    Message = "Voucher deleted successfully",
                    Data = new DeleteVoucherResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteVoucherResponse>
                {
                    Code = 500,
                    Message = $"Error deleting voucher: {ex.Message}"
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
                    Message = $"Error updating expired vouchers: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<List<GetVoucherByIdResponse>>> GetNonExpiredVouchers()
        {
            try
            {
                var vouchers = await _voucherRepository.GetNonExpiredVouchersAsync();
                var voucherResponses = vouchers.Select(voucher => new GetVoucherByIdResponse()
                {
                    Id = voucher.Id,
                    Code = voucher.VoucherName ?? string.Empty,
                    DiscountAmount = float.TryParse(voucher.VoucherValue, out float value) ? value : 0,
                    Status = voucher.Status.ToString(),
                    ExpiryDate = voucher.ExpirationDate,
                    IsUsed = voucher.Orders != null && voucher.Orders.Any(),
                    OrderIds = voucher.Orders?.Select(o => o.Id).ToList() ?? new List<long>()
                }).ToList();

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
                    Message = $"Error retrieving non-expired vouchers: {ex.Message}"
                };
            }
        }

        private void ValidateExpirationDate(DateTime expiryDate, DateTime? createdAt = null)
        {
            var now = DateTime.UtcNow;
            createdAt ??= now;

            if (expiryDate < now)
                throw new ArgumentException("Expiration date cannot be in the past");

            var maxExpiryDate = createdAt.Value.AddDays(MAX_EXPIRY_DAYS);
            if (expiryDate > maxExpiryDate)
                throw new ArgumentException($"Expiration date cannot be more than {MAX_EXPIRY_DAYS} days from creation date");
        }
    }
}