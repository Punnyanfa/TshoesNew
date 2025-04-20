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
  
export async function createTemplate(templateData) {
  try {
    console.log('Request data:', Object.fromEntries(templateData.entries()));
    
    const response = await instance.post('/Template', templateData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Accept': 'application/json'
      }
    });
    
    // Check if response code is 200
    if (response.data.code === 200) {
      return response.data;
    }
    
    // If code is not 200, throw error
    throw new Error(response.data.message || 'Failed to create template');
  } catch (error) {
    console.error("Error creating template:", error.response?.data || error.message);
    console.error("Status:", error.response?.status);
    console.error("Headers:", error.response?.headers);
    throw error;
  }
}
  
export async function updateTemplate(id, templateData) {
  try {
    const response = await instance.put('/Template', {
      id: id,
      name: templateData.name,
      description: templateData.description,
      gender: templateData.gender,
      color: templateData.color,
      previewImage: templateData.previewImage,
      model3DFile: templateData.model3DFile,
      basePrice: templateData.basePrice,
      isAvailable: templateData.isAvailable
    });
    
    // Check if response code is 200
    if (response.data.code === 200) {
      return response.data;
    }
    
    // If code is not 200, throw error
    throw new Error(response.data.message || 'Failed to update template');
  } catch (error) {
    console.error("Error updating template:", error);
    throw error;
  }
}
  
export async function deleteTemplate(id) {
  try {
    const response = await instance.delete(`/Template/${id}`);
    
    // Check if response code is 200
    if (response.data.code === 200) {
      return response.data;
    }
    
    // If code is not 200, throw error
    throw new Error(response.data.message || 'Failed to delete template');
  } catch (error) {
    console.error("Error deleting template:", error);
    throw error;
  }
}
  
