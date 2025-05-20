using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Voucher;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var voucher = CreateVoucherFromRequest(request);
                var addedVoucher = await _voucherRepository.AddAsync(voucher);
                return new BaseResponseModel<AddVoucherResponse>
                {
                    Code = 201,
                    Message = "Voucher created successfully",
                    Data = new AddVoucherResponse { VoucherId = addedVoucher.Id }
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
                var voucher = await GetVoucherEntityById(new GetVoucherByIdRequest { Id = request.Id });
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
                var voucher = await GetVoucherEntityById(new GetVoucherByIdRequest { Id = request.Id });
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

        private Voucher CreateVoucherFromRequest(AddVoucherRequest request)
        {
            ValidateExpirationDate(request.ExpiryDate);
            return new Voucher
            {
                VoucherName = request.Code,
                VoucherValue = request.DiscountAmount.ToString(),
                ExpirationDate = request.ExpiryDate,
                Status = (int)VoucherStatus.Active,
                Description = request.Code,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private void UpdateVoucherFromRequest(Voucher voucher, UpdateVoucherRequest request)
        {
            ValidateExpirationDate(request.ExpiryDate, voucher.CreatedAt);
            voucher.VoucherName = request.Code;
            voucher.VoucherValue = request.DiscountAmount.ToString();
            voucher.ExpirationDate = request.ExpiryDate;
            voucher.UpdatedAt = DateTime.UtcNow;
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
            var now = DateTime.UtcNow;
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