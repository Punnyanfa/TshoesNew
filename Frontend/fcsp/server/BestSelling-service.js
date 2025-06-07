import { instance, BestSelling } from "./api-instance-provider";

// Get all products

export async function GetBestSelling() {
    try {
      const response = await instance.get(BestSelling.ORIGIN);
      // Kiểm tra mã phản hồi và trả về toàn bộ phản hồi nếu mã là 200
      if (response.data.code === 200) {
        return response.data;  // 
      }
      
      // Nếu mã không phải 200, throw lỗi
      throw new Error(response.data.message || 'Failed to fetch products');
    } catch (error) {
      console.error("Error getting products:", error);
      throw error;
    }
  }
  