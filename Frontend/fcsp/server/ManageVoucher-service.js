import { instance } from "./api-instance-provider";



export async function getAllVouchers() {
  try {
    const response = await instance.get(`/Voucher`);
    if (response.data.code === 200) {
      return response.data.data;  
    }
    throw new Error(response.data.message || `Failed to fetch voucher`);
  } catch (error) {
    console.error(`Error getting voucher:`, error);
    throw error;
  }
}