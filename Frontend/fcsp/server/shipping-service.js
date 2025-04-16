import { instance } from "./api-instance-provider";


export async function getAllShippingInfo() {
  try {
    const response = await instance.get(`/ShippingInfo`);
    if (response.data.code === 200) {
      return response.data.data;
    }
    throw new Error(response.data.message || `Failed to fetch shipping info`);
  } catch (error) {
    console.error(`Error getting shipping info:`, error);
    throw error;
  }
}
export async function postShippingInfo(shippingData) {
    try {
      const payload = {
        userId: shippingData.userId,
        phoneNumber: shippingData.phoneNumber,
        address: shippingData.address,
        city: shippingData.city,
        district: shippingData.district,
        ward: shippingData.ward,
        country: shippingData.country || "Vietnam", 
        isDefault: shippingData.isDefault
      };
      if (shippingData.id) {
        payload.id = shippingData.id;
      }
      const response = await instance.post('/ShippingInfo', payload);
     
      if (response.data && response.data.code === 200) {
        return response.data.data;
      }
      
      throw new Error(response.data?.message || 'Failed to save shipping info');
    } catch (error) {
      console.error('Error in postShippingInfo:', error);
      throw error;
    }
}

export async function shippingInfo(userId) {
    try {
      const response = await instance.get(`/ShippingInfo/user/${userId}`);
      
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
      
      if (response.data && response.data.code === 200) {
        return true;
      }
      
      throw new Error(response.data?.message || 'Failed to delete shipping info');
    } catch (error) {
      console.error('Error deleting shipping info:', error);
      throw error;
    }
}