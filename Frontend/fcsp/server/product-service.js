import { instance } from "./api-instance-provider";

// Get all products

export async function getAllProducts() {
    try {
      const response = await instance.get('/CustomShoeDesign');
      
      // Kiểm tra mã phản hồi và trả về toàn bộ phản hồi nếu mã là 200
      if (response.data.code === 200) {
        return response.data;  // Trả về toàn bộ đối tượng chứa 'designs' và các thông tin khác
      }
      
      // Nếu mã không phải 200, throw lỗi
      throw new Error(response.data.message || 'Failed to fetch products');
    } catch (error) {
      console.error("Error getting products:", error);
      throw error;
    }
  }
  

// Get product by ID
export async function getProductById(id) {
  try {
    const response = await instance.get(`/CustomShoeDesign/${id}`);
    if (response.data.code === 200) {
      return response.data.data;  // Trả về data của sản phẩm cụ thể
    }
    throw new Error(response.data.message || `Failed to fetch product with id ${id}`);
  } catch (error) {
    console.error(`Error getting product with id ${id}:`, error);
    throw error;
  }
}

export async function updateProductStatus(id, status) {
  try {
    const response = await instance.put(`/CustomShoeDesign/status`, {
      id: id,
      status: status
    });
    
    if (response.data.code === 200) {
      return response.data;
    }
    
    throw new Error(response.data.message || 'Failed to update product status');
  } catch (error) {
    console.error("Error updating product status:", error);
    throw error;
  }
}

// Delete product by ID
export async function deleteProduct(id) {
  try {
    const response = await instance.delete(`/CustomShoeDesign/${id}`);
    if (response.data.code === 200) {
      return response.data;
    }
    throw new Error(response.data.message || `Failed to delete product with id ${id}`);
  } catch (error) {
    console.error(`Error deleting product with id ${id}:`, error);
    throw error;
  }
}