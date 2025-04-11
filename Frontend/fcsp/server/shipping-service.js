import { instance } from "./api-instance-provider";

export async function postShippingInfo(shippingData) {
    console.log("shippingData",shippingData);
    try {
      // Đảm bảo dữ liệu gửi lên đúng format
      const payload = {
        userId: shippingData.userId,
        phoneNumber: shippingData.phoneNumber,
        address: shippingData.address,
        city: shippingData.city,
        district: shippingData.district,
        ward: shippingData.ward,
        country: shippingData.country || "Vietnam", // Mặc định là Vietnam nếu không có
        isDefault: shippingData.isDefault
      };
      // Nếu đang edit thì thêm id vào payload
      if (shippingData.id) {
        payload.id = shippingData.id;
      }
      const response = await instance.post('/ShippingInfo', payload);
     
      // Kiểm tra response và trả về dữ liệu
      if (response.data && response.data.code === 200) {
        return response.data.data;
      }
      
      // Nếu có lỗi từ API
      throw new Error(response.data?.message || 'Failed to save shipping info');
    } catch (error) {
      console.error('Error in postShippingInfo:', error);
      throw error;
    }
}

export async function shippingInfo(userId) {
    try {
      // Gọi API với userId
      const response = await instance.get(`/ShippingInfo/user/${userId}`);
      
      // Kiểm tra response và trả về dữ liệu
      if (response.data && response.data.code === 200) {
        return response.data.data;
      }
      
      throw new Error(response.data?.message || `Failed to fetch shipping info for user ${userId}`);
    } catch (error) {
      console.error(`Error getting shipping info for user ${userId}:`, error);
      throw error;
    }
}

export async function deleteShippingInfo(id) {
    try {
      const response = await instance.delete(`/ShippingInfo/${id}`);
      
      // Kiểm tra response
      if (response.data && response.data.code === 200) {
        return true;
      }
      
      throw new Error(response.data?.message || 'Failed to delete shipping info');
    } catch (error) {
      console.error('Error deleting shipping info:', error);
      throw error;
    }
}