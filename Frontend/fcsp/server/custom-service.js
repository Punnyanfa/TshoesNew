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
  
export async function addTemplate(templateData) {
  try {
    // Log FormData entries for debugging
    console.log('Sending template data:');
    for (let [key, value] of templateData.entries()) {
      if (value instanceof File) {
        console.log(key + ':', {
          name: value.name,
          type: value.type,
          size: value.size
        });
      } else {
        console.log(key + ':', value);
      }
    }

    const response = await instance.post('/Template', templateData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Accept': '*/*'
      },
      transformRequest: [(data) => data]
    });
    
    // Check if response contains data
    if (response.data) {
      return response.data;
    }
    throw new Error('No response data received');
  } catch (error) {
    // Only throw error if it's a real error, not a success message
    if (error.message && error.message.toLowerCase().includes('successfully')) {
      return { code: 200, message: error.message };
    }
    console.error("Error adding template:", error.response?.data || error.message);
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
  
