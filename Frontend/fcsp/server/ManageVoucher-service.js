import { instance } from "./api-instance-provider";

export async function getAllVouchers() {
  try {
    const response = await instance.get(`/Voucher`);
    return response.data;
  } catch (error) {
    console.error(`Error getting vouchers:`, error);
    throw error;
  }
}

export async function postVoucher(voucherData) {
  try {
    const requestBody = {
      code: voucherData.code,
      discountAmount: Number(voucherData.discountAmount),
      expiryDate: voucherData.expiryDate
    };
    
    const response = await instance.post('/Voucher', requestBody);
    return response.data;
  } catch (error) {
    console.error('Error creating voucher:', error);
    throw error;
  }
}

export async function deleteVoucher(id) {
  try {
    const response = await instance.delete(`/Voucher/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting voucher:', error);
    throw error;
  }
}
