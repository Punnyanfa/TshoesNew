import { instance } from "./api-instance-provider";

// Get all products

export async function getAllTemplate() {
    try {
      const response = await instance.get('/Template');
      
      // Kiểm tra mã phản hồi và trả về toàn bộ phản hồi nếu mã là 200
      if (response.data.code === 200) {
        return response.data;  // Trả về toàn bộ đối tượng chứa 'designs' và các thông tin khác
      }
      
      // Nếu mã không phải 200, throw lỗi
      throw new Error(response.data.message || 'Failed to fetch template');
    } catch (error) {
      console.error("Error getting template:", error);
      throw error;
    }
  }
  
